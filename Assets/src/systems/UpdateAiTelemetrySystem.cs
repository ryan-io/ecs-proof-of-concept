// src

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src.systems {
	[BurstCompile]
	public partial struct UpdateAiTelemetrySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PlayerComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
			var playerTr     = SystemAPI.GetComponentRO<LocalToWorld>(playerEntity);

			var updateTelemetryJob = new UpdateAiTelemetryJob {
				PlayerPosition = playerTr.ValueRO.Position,
			};

			var handle = updateTelemetryJob.ScheduleParallel(state.Dependency);
			handle.Complete();
		}
	}

	[BurstCompile]
	public partial struct UpdateAiTelemetryJob : IJobEntity {
		public float3 PlayerPosition;

		[BurstCompile]
		public void Execute(AiTelemetryAspect telemetry) {
			telemetry.SetTelemetryHeading(PlayerPosition);
			telemetry.SetTelemetrySqDistToPlayer(PlayerPosition);
		}
	}
}