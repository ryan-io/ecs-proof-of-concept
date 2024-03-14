// src

using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace src {
	[BurstCompile]
	public partial struct SetInitialPositionAiTelemetrySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<AiTelemetry>();
			state.RequireForUpdate<LocalToWorld>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Enabled = false;

			foreach (var (tr, aiTelemetry, e) in
			         SystemAPI.Query<RefRO<LocalTransform>, RefRW<AiTelemetry>>()
			                  .WithEntityAccess()) {
				aiTelemetry.ValueRW.InitialPosition = tr.ValueRO.Position;
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}