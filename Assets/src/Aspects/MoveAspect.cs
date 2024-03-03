// src

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src {
	public readonly partial struct MoveAspect : IAspect {
		public bool IsInHostileRange()
			=> m_telemetry.ValueRO.SqDistanceToPlayer <= m_hostileDistanceSq.ValueRO.DistanceSq;

		public float3 Heading => m_telemetry.ValueRO.HeadingToPlayer;
		
		public void MoveTowardsPlayer(float speed, float deltaTime) {
			var vectorDelta = speed * Heading * deltaTime;
			m_transform.ValueRW.Position += vectorDelta;
		}

		public void MoveAwayFromPlayer(float speed, float deltaTime) {
			var vectorDelta = -speed * Heading * deltaTime;
			m_transform.ValueRW.Position += vectorDelta;
		}

		readonly EnabledRefRO<MoveableEntityTag>       m_moveableEntityTag;
		readonly RefRW<LocalTransform>                 m_transform;
		readonly RefRO<DistanceBeforeHostileComponent> m_hostileDistanceSq;
		readonly RefRO<AiTelemetry>                    m_telemetry;
	}
}