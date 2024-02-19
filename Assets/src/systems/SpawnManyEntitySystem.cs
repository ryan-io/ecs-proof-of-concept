using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace src.systems {
	[BurstCompile]
	public partial struct SpawnManyEntitySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<SpawnManyEntityData>();
			state.RequireForUpdate<RandomNumGenData>();
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Enabled = false;

			var sysSing = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var ecb     = sysSing.CreateCommandBuffer(state.WorldUnmanaged);

			var spawnData = SystemAPI.GetSingleton<SpawnManyEntityData>();
			var rndData   = SystemAPI.GetSingleton<RandomNumGenData>();

			var cellSize        = spawnData.SeparationRadius / math.SQRT2;
			var width           = (int)math.ceil(spawnData.RegionWidth  / cellSize) + 1;
			var height          = (int)math.ceil(spawnData.RegionHeight / cellSize) + 1;
			var cellScalar      = (int)math.ceil(1 / cellSize);
			var outputPositions = new NativeList<float3>(cellScalar * width * height, Allocator.Persistent);

			var positionJob = new CalculateSpawnPositions {
				FinalPositions          = outputPositions,
				SpawnCount              = spawnData.Count,
				Random                  = rndData.Value,
				IterationsBeforeDiscard = spawnData.IterationsBeforeDiscard,
				SeparationRadius        = spawnData.SeparationRadius,
				CellSize                = cellSize,
				Width                   = width,
				Height                  = height
			};

			var positionJobHandle = positionJob.Schedule();
			positionJobHandle.Complete();

			var instantiationJob = new SpawnManyEntityJob {
				ParallelWriter = ecb.AsParallelWriter(),
				Positions      = outputPositions.AsArray()
			};

			// we can also write:  instantiationJob.ScheduleParallelByRef() -> same as below (source-gen)
			var instantiateJobHandle = instantiationJob.ScheduleParallelByRef(state.Dependency);
			instantiateJobHandle.Complete();
			state.Dependency = instantiateJobHandle;
			outputPositions.Dispose();
		}
	}

	[BurstCompile]
	public struct CalculateSpawnPositions : IJob {
		public NativeList<float3> FinalPositions;
		public Random             Random;
		public int                SpawnCount;
		public int                IterationsBeforeDiscard;
		public float              CellSize;
		public float              SeparationRadius;
		public int                Width;
		public int                Height;

		[BurstCompile]
		public void Execute() {
			// https: //sighack.com/post/poisson-disk-sampling-bridsons-algorithm

			var grid            = new NativeArray<float3>(65356, Allocator.Temp);
			var activeList      = new NativeList<float3>(65356, Allocator.Temp);

			var p0X = Random.NextFloat(Width);
			var p0Y = Random.NextFloat(Height);
			var p0  = new float3(p0X, p0Y, 0);
			Insert(ref grid, Width, CellSize, p0);
			FinalPositions.Add(p0);
			activeList.Add(p0);

			int counter = 0;

			do {
				counter++;
				var index   = Random.NextInt(activeList.Length - 1);
				var point   = activeList[index];
				var success = false;

				for (var i = 0; i < IterationsBeforeDiscard; i++) {
					var angle        = math.radians(Random.NextFloat(360));
					var randomRadius = Random.NextFloat(SeparationRadius, 2 * SeparationRadius);

					var coordX = point.x + randomRadius * math.cos(angle);
					var coordY = point.x + randomRadius * math.sin(angle);

					var testPoint = new float3(coordX, coordY, 0);

					if (!IsValid(ref grid, Width, Width, Height, testPoint, SeparationRadius, CellSize))
						continue;

					FinalPositions.Add(testPoint);
					activeList.Add(testPoint);
					Insert(ref grid, Width, CellSize, testPoint);
					success = true;
					break;
				}

				if (!success) {
					activeList.RemoveAt(index);
					activeList.SetCapacity(activeList.Length);
				}
			} while (counter < SpawnCount);

			grid.Dispose();
			activeList.Dispose();
		}

		[BurstCompile]
		void Insert(ref NativeArray<float3> grid, int numOfCol, float cellSize, float3 worldPoint) {
			int rowIndex    = (int)math.floor(worldPoint.x / cellSize); // x
			int colIndex    = (int)math.floor(worldPoint.y / cellSize); // y
			int insertIndex = rowIndex * numOfCol + colIndex;
			grid[insertIndex] = worldPoint;
		}

		[BurstCompile]
		float3 Get(ref NativeArray<float3> grid, int numOfCol, int rowIndex, int colIndex) {
			int insertIndex = rowIndex + numOfCol * colIndex;
			return grid[insertIndex];
		}

		[BurstCompile]
		bool IsValid(ref NativeArray<float3> grid, int numOfCol, int width, int height, float3 point, float radius,
			float
				cellSize) {
			if (point.x < 0 || point.y < 0 || point.x >= width || point.y >= height)
				return false;

			int xCheck = (int)math.floor(point.x / cellSize);
			int yCheck = (int)math.floor(point.y / cellSize);
			int xLower = math.max(xCheck - 1, 0);
			int xUpper = math.max(xCheck + 1, width - 1);
			int yLower = math.max(yCheck - 1, 0);
			int yUpper = math.max(xCheck + 1, height - 1);

			for (var row = xLower; row <= xUpper; row++) {
				for (var col = yLower; col <= yUpper; col++) {
					var value  = Get(ref grid, numOfCol, row, col);
					var sqDist = math.distancesq(point, value);

					if (sqDist < radius * radius)
						return false;
				}
			}

			return true;
		}
	}

	[BurstCompile]
	public partial struct SpawnManyEntityJob : IJobEntity {
		public            EntityCommandBuffer.ParallelWriter ParallelWriter;
		[ReadOnly] public NativeArray<float3>                Positions;

		[BurstCompile]
		public void Execute([ChunkIndexInQuery] int chunkIndex, ref SpawnManyEntityData data) {
			s_preparePerfMarker.Begin();

			// Allocator.Temp is automatically disposed
			var instances = new NativeArray<Entity>(Positions.Length, Allocator.Temp);

			ParallelWriter.Instantiate(chunkIndex, data.Entity, instances);

			for (int i = 0; i < Positions.Length; i++) {
				ParallelWriter.SetComponent(chunkIndex, instances[i], new LocalTransform {
					Position = Positions[i], Rotation = quaternion.identity, Scale = 1f
				});
			}

			s_preparePerfMarker.End();
		}

		static readonly ProfilerMarker s_preparePerfMarker = new("POF-SpawnEntity");
	}
}