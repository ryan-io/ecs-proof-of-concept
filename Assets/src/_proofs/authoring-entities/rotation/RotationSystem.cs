// // Assembly-CSharp
//
// using Unity.Assertions;
// using Unity.Burst;
// using Unity.Burst.Intrinsics;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Transforms;
//
// namespace src {
// 	[BurstCompile]
// 	public partial struct RotationSystem : ISystem {
// 		[BurstCompile]
// 		public void OnCreate(ref SystemState state) {
// 			m_transformHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>();
// 			m_rotationDataHandle = SystemAPI.GetComponentTypeHandle<RotationData>(true);
// 		}
// 		
// 		// data -> delta time & speed
// 		[BurstCompile]
// 		public void OnUpdate(ref SystemState state) {
// 			var dT = SystemAPI.Time.DeltaTime;
//
// 			// foreach (var aspect in SystemAPI.Query<RotationAspect>()) {
// 			// 	aspect.Rotate(dT);
// 			// }
//
// 			var job = new RotationJob { DeltaTime = SystemAPI.Time.DeltaTime };
// 			job.ScheduleParallel();
//
// 			var chunkQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, RotationData>().Build();
// 			
// 			m_rotationDataHandle.Update(ref state);
// 			m_transformHandle.Update(ref state);
// 			
// 			var chunkJob = new RotationJobChunk {
// 				TransformHandle    = m_transformHandle,
// 				RotationDataHandle = m_rotationDataHandle,
// 				DeltaTime = SystemAPI.Time.DeltaTime
// 			};
//
// 			state.Dependency = chunkJob.ScheduleParallel(chunkQuery, state.Dependency);
// 		}
//
// 		// it is recommended to cache ComponentTypeHandles in OnCreate and invoke Update on the handles
// 		ComponentTypeHandle<LocalTransform> m_transformHandle;
// 		ComponentTypeHandle<RotationData>   m_rotationDataHandle;
// 	}
//
// 	[BurstCompile]
// 	public partial struct RotationJob : IJobEntity {
// 		public float DeltaTime;
//
// 		[BurstCompile]
// 		public void Execute(ref RotationData data, ref LocalTransform tr) {
// 			tr = tr.RotateZ(data.RotationSpeed * DeltaTime);
// 		}
// 	}
//
//
// 	// modifying entity-component values via IJobChunk
// 	// again -> this MODIFIES (ONLY) entity component values directly
// 	
// 	// take care to not schedule two or more jobs that WRITE to the same chunk of memory (RotationData)
// 	// this is perfectly okay if the chunk is marked as ReadOnly
//
// 	[BurstCompile]
// 	public struct RotationJobChunk : IJobChunk {
// 		public            ComponentTypeHandle<LocalTransform> TransformHandle;
// 		[ReadOnly] public ComponentTypeHandle<RotationData>   RotationDataHandle;
// 		[ReadOnly] public float                               DeltaTime;
//
// 		public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask,
// 			in v128 chunkEnabledMask) {
//
// 			// this filter out any components that are NOT enabled
// 			Assert.IsFalse(useEnabledMask);
//
// 			var trs    = chunk.GetNativeArray(ref TransformHandle);
// 			var rotationData = chunk.GetNativeArray(ref RotationDataHandle);
//
// 			for (var i = 0;  i < chunk.Count; i++) {
// 				trs[i] = trs[i].Translate(rotationData[i].RotationSpeed * DeltaTime);
// 			}
// 		}
// 	}
//
// 	[BurstCompile]
// 	readonly partial struct RotationAspect : IAspect {
// 		[BurstCompile]
// 		public void Rotate(float deltaTime) {
// 			m_transform.ValueRW = m_transform.ValueRW.RotateZ(
// 				m_data.ValueRO.RotationSpeed * deltaTime);
// 		}
//
// 		readonly RefRW<LocalTransform> m_transform;
// 		readonly RefRO<RotationData>   m_data;
// 	}
// }