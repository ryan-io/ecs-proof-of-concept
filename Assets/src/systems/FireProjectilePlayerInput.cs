using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src.systems {
	[BurstCompile]
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial struct FireProjectilePlayerInput : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PlayerWeakAttackInputTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var ecb          = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

			foreach (var (tr, tag, projData, entity) in
			         SystemAPI.Query<
				         RefRO<LocalTransform>,
				         RefRO<PlayerWeakAttackInputTag>,
				         RefRO<ProjectileComponent>>()
			                  .WithEntityAccess()) {
				var e = ecb.Instantiate(projData.ValueRO.Prefab);

				ecb.SetComponent(e, new LocalTransform {
					Position = tr.ValueRO.Position,
					Rotation = quaternion.identity,
					Scale    = 1.0f
				});
				
				ecb.SetComponentEnabled<PlayerWeakAttackInputTag>(entity, false);
			}
		}
	}
}