// // Assembly-CSharp
//
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
//
// namespace src {
// 	[BurstCompile]
// 	public partial struct SpawnSystem : ISystem {
// 		[BurstCompile]
// 		public void OnCreate(ref SystemState state) {
// 			// Generic of RequireForUpdate<T> should IComponentData
// 			state.RequireForUpdate<SpawnerComponentData>();
//
// 			// another required component for running OnUpdate logic
// 			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
// 		}
//
// 		[BurstCompile]
// 		public void OnUpdate(ref SystemState state) {
// 			// to only run OnUpdate once (put this at the beginning of method):
// 			// state.Enabled = false;
//             
// 			// this is important -> EntityCommandBufferSystem AUTOMATICALLY plays back commands
// 			// IF you do not use this, you manually have to invoke Playback and provide an EntityManager
// 			//		see DeparentSystem below
// 			
// 			// documentation ->
// 			// https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-entity-command-buffer-automatic-playback.html
// 			var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
// 			var ecb          = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
//
// 			var job = new SpawnIfApplicableJob {
// 				EcbParaWriter = ecb.AsParallelWriter(),
// 				ElapsedTime   = (float)SystemAPI.Time.ElapsedTime,
// 			};
//
// 			var r = Random.CreateFromIndex((uint)SystemAPI.Time.ElapsedTime);
//
// 			foreach (var (tr, spawnSys) in
// 			         SystemAPI.Query<RefRW<LocalTransform>, RefRO<SpawnerComponentData>>()) {
// 				var constructorValues = r.NextFloat3(-100, 100);
// 				tr.ValueRW.Position = constructorValues;
// 			}
//
// 			job.ScheduleParallel();
// 		}
//
// 		[BurstCompile]
// 		public partial struct SpawnIfApplicableJob : IJobEntity {
// 			public            EntityCommandBuffer.ParallelWriter EcbParaWriter;
// 			[ReadOnly] public float                              ElapsedTime;
//
// 			[BurstCompile]
// 			public void Execute([ChunkIndexInQuery] int index, ref SpawnerComponentData data) {
// 				if (ElapsedTime < data.NextSpawnTime)
// 					return;
//
// 				// allows us to modify entity components (which has to be done on the main thread)
// 				// Ecb allows us to register Commands to be invoked at a later time on the main thread
// 				//		creating/destroying entities, setting component values
// 				EcbParaWriter.Instantiate(index, data.Prefab);
// 				data.NextSpawnTime += data.SpawnDelay;
// 			}
// 		}
// 		
//
//
// 		[BurstCompile]
// 		public partial struct DeparentSystem : ISystem {
// 			public void OnCreate(ref SystemState state) {
// 				state.RequireForUpdate<SpawnerComponentData>();
// 			}
//
// 			[BurstCompile]
// 			public void OnUpdate(ref SystemState state) {
// 				var entity = SystemAPI.GetSingletonEntity<SpawnerComponentData>();
// 				var ecb    = new EntityCommandBuffer(Allocator.Temp);
//
// 				//DynamicBuffer<Child> childComponents = SystemAPI.GetBuffer<Child>(entity);
//
// 				// if (childComponents.Length < 1)
// 				// 	return;
// 				//
// 				// // this is required because we do NOT use EntityCommandBufferSystem
// 				// ecb.Playback(state.EntityManager);
// 				//
// 				// // if we want to get an entity via a SystemAPI.Query, invoke 'WithEntityAccess()'
// 				// foreach (var (tr, e) in SystemAPI.Query<RefRO<LocalTransform>>()
// 				//                                       .WithNone<RotationData>()
// 				//                                       .WithEntityAccess()) {
// 				// 	
// 				// }
// 			}
// 		}
// 	}
// }