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
			var playerTr     = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

			var ecb    = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var writer = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
			
			foreach (var localTransform in SystemAPI.Query<RefRO<LocalTransform>>()
			                                        .WithAll<AiTelemetry>()) {
				var updateTelemetryJob = new UpdateAiTelemetryJob {
					PlayerPosition = playerTr.ValueRO.Position,
					AiPosition     = localTransform.ValueRO.Position
				};

				var handle = updateTelemetryJob.ScheduleParallel(state.Dependency);
				handle.Complete();
			}
		}
	}

	[BurstCompile]
	public partial struct UpdateAiTelemetryJob : IJobEntity {
		public float3                             PlayerPosition;
		public float3                             AiPosition;

		[BurstCompile]
		public void Execute(AiTelemetryAspect telemetry) {
			var sqDist  = math.distancesq(AiPosition, PlayerPosition);
			var heading =  math.normalizesafe(AiPosition - PlayerPosition);
			
			telemetry.SetTelemetryHeading(heading);
			telemetry.SetTelemtrySqDistToPlayer(sqDist);
		}
	}
}