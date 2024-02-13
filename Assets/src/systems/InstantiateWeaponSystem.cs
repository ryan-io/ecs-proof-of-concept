using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src.systems {
	[UpdateInGroup(typeof(InitializationSystemGroup)), BurstCompile]
	public partial struct InstantiateWeaponSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerData>();
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Enabled = false;

			var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var ecb          = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

			foreach (var (weaponData, trAttachData) in
			         SystemAPI.Query<RefRO<EquippableData>, RefRO<WeaponAttachmentData>>()) {
				var e = ecb.Instantiate(weaponData.ValueRO.Prefab);

				ecb.AddComponent(e, new Parent {
					Value = trAttachData.ValueRO.Pivot
				});

				ecb.SetComponent(e, new LocalTransform {
					Position = trAttachData.ValueRO.Offset,
					Rotation = quaternion.identity,
					Scale    = 1.0f
				});
			}
		}
	}
}