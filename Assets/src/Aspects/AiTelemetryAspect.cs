using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public readonly partial struct AiTelemetryAspect : IAspect {
		public readonly Entity Self;

		public void SetTelemetryHeading(float3 heading) => m_telemetry.ValueRW.HeadingToPlayer = heading;

		public void SetTelemtrySqDistToPlayer(float distance) => m_telemetry.ValueRW.SqDistanceToPlayer = distance;
		
		readonly RefRW<AiTelemetry> m_telemetry;
	}
}