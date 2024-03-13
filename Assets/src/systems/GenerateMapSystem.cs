// src

using System.Globalization;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace src {
	[BurstCompile]
	public partial struct GenerateMapSystem : ISystem {
		NativeArray<float3> m_positions;

		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<MapComponent>();
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
			m_positions.Dispose();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Enabled = false;
			var mapData = SystemAPI.GetSingleton<MapComponent>();

			m_positions = new NativeArray<float3>(mapData.Height * mapData.Width, Allocator.Persistent);

			// for (var i = 0; i < m_randoms.Length; i++) {
			// 	m_randoms[i] = new Random(rndSeed.Value.NextUInt());
			// }

			var calcPosJob = new CalculatePositionsJob {
				Positions = m_positions,
				TileSize  = MapComponent.TILE_SIZE,
				Width     = mapData.Width,
				Height    = mapData.Height
			};

			var calcPosHandle = calcPosJob.Schedule(state.Dependency);
			calcPosHandle.Complete();

			var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var ecb          = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

			var createTerrainJob = new CreateTerrainJob {  
				Positions = m_positions,
				Ecb       = ecb.AsParallelWriter(),
				Terrain   = mapData.Terrain
			};

			state.Dependency = createTerrainJob.Schedule(m_positions.Length, 64, calcPosHandle);
		}

		[BurstCompile]
		public struct CalculatePositionsJob : IJob {
			public            NativeArray<float3> Positions;
			[ReadOnly] public float               TileSize;
			[ReadOnly] public int                 Width;
			[ReadOnly] public int                 Height;

			[BurstCompile]
			public void Execute() {
				for (var width = 0; width < Width; width++) {
					for (var height = 0; height < Height; height++) {
						var i         = GetIndex(Height, width, height);
						var unscaledV = new float3(height, width, 0);
						Positions[i] = TileSize * unscaledV;
					}
				}
			}

			[BurstCompile]
			static int GetIndex(int gridHeight, int rowIndex, int colIndex) {
				return colIndex + gridHeight * rowIndex;
			}
		}

		[BurstCompile]
		public struct CreateTerrainJob : IJobParallelFor {
			[ReadOnly] public NativeArray<float3>                Positions;
			public            EntityCommandBuffer.ParallelWriter Ecb;
			public            Entity                             Terrain;
			
			[BurstCompile]
			public void Execute(int index) {
				var terrain = Ecb.Instantiate(index, Terrain);
				Ecb.SetComponent(index, terrain, LocalTransform.FromPosition(Positions[index]));
			}
		} 
	}
}