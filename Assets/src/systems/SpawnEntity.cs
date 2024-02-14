using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Profiling;

namespace src.systems {
	[BurstCompile]
	public partial struct SpawnEntity : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Enabled = false;

			var sysSing          = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var ecb              = sysSing.CreateCommandBuffer(state.WorldUnmanaged);
			var instantiationJob = new SpawnEntityJob { ParallelWriter = ecb.AsParallelWriter() };

			state.Dependency = instantiationJob.ScheduleParallelByRef(state.Dependency);
		}
	}

	[BurstCompile]
	public partial struct SpawnEntityJob : IJobEntity {
		public EntityCommandBuffer.ParallelWriter ParallelWriter;

		[BurstCompile]
		public void Execute([ChunkIndexInQuery] int chunkIndex, ref SpawnEntityData data) {
			s_preparePerfMarker.Begin();
			var instances = new NativeArray<Entity>(data.Count, Allocator.Temp);
			ParallelWriter.Instantiate(chunkIndex, data.Entity, instances);
			s_preparePerfMarker.End();
		}
		
		static readonly ProfilerMarker s_preparePerfMarker = new ("POF-SpawnEntity");
	}
}