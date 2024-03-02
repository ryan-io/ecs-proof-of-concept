using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src.systems {
	[UpdateInGroup(typeof(SimulationSystemGroup)), BurstCompile]
	public partial struct LookAtPlayerSystem : ISystem {
		//ComponentLookup<AiSpawnComponent> m_testLookup;

		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerComponent>();
			//m_testLookup = SystemAPI.GetComponentLookup<AiSpawnComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var playerEntity  = SystemAPI.GetSingleton<PlayerComponent>().Entity;
			var playerLocalTr = SystemAPI.GetComponent<LocalTransform>(playerEntity);

			var job = new LookAtPlayerJob {
				PlayerTr = playerLocalTr
			};

			var handle = job.ScheduleParallel(state.Dependency);
			handle.Complete();
		}
	}

	[BurstCompile]
	public partial struct LookAtPlayerJob : IJobEntity {
		public LocalTransform PlayerTr;

		public void Execute(LookAtPlayerAspect aspect) {
			var ownerPos  = aspect.LocalToWorldPosition;
			var playerPos = PlayerTr.Position;

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