using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src.systems {
	[UpdateInGroup(typeof(SimulationSystemGroup)), BurstCompile]
	public partial struct LookAtPlayerSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var playerEntity  = SystemAPI.GetSingletonEntity<PlayerComponent>();
			var playerLocalTr = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

			var job = new LookAtPlayerJob {
				PlayerPosition = playerLocalTr.ValueRO.Position
			};

			var handle = job.ScheduleParallel(state.Dependency);
			handle.Complete();
		}
	}
	
	[BurstCompile]
	public partial struct LookAtPlayerJob : IJobEntity {
		public float3 PlayerPosition;

		[BurstCompile]
		public void Execute(LookAtPlayerAspect aspect) {
			var ownerPos  = aspect.LocalToWorldPosition;
			var playerPos = PlayerPosition;

			int2 flipVector = new int2();

			if (ownerPos.x <= playerPos.x) {
				flipVector.x = 1;
			}
			else {
				flipVector.x = -1;
			}

			if (ownerPos.y < playerPos.y) {
				flipVector.y = 1;
			}
			else {
				flipVector.y = -1;
			}

			aspect.SetFlip(flipVector);
		}
	}
}