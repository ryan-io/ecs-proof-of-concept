using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src {
	[BurstCompile]
	public readonly partial struct AiTelemetryAspect : IAspect {
		public readonly Entity Self;

		[BurstCompile]
		public void SetHeadingToPlayer(float3 playerPos) {
			var vector = math.normalizesafe(playerPos - m_tr.ValueRO.Position);
			m_telemetry.ValueRW.HeadingToPlayer = new float3(vector.x, vector.y, 0);
		}

		[BurstCompile]
		public void SetHeadingToHome() {
			var vector = math.normalizesafe(m_telemetry.ValueRO.InitialPosition - m_tr.ValueRO.Position);
			m_telemetry.ValueRW.HeadingToHome = new float3(vector.x, vector.y, 0);
		}

		[BurstCompile]
		public void SetSqDistToHome() {
			m_telemetry.ValueRW.SqDistanceToHome = math.distancesq(
				m_tr.ValueRO.Position, m_telemetry.ValueRO.InitialPosition);
		}

		[BurstCompile]
		public void SetSqDistToPlayer(float3 playerPos) {
			m_telemetry.ValueRW.SqDistanceToPlayer = math.distancesq(m_tr.ValueRO.Position, playerPos);
		}

		readonly RefRW<AiTelemetry>  m_telemetry;
		readonly RefRW<LocalToWorld> m_tr;
	}
}