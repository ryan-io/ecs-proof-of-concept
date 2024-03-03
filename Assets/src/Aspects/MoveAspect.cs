// src

using Unity.Entities;
using Unity.Transforms;

namespace src {
	public readonly partial struct MoveAspect : IAspect {
		public bool IsInHostileRange()
			=> m_telemetry.ValueRO.SqDistanceToPlayer <= m_hostileDistanceSq.ValueRO.DistanceSq;

		readonly EnabledRefRO<MoveableEntityTag>       m_moveableEntityTag;
		readonly RefRW<LocalTransform>                 m_transform;
		readonly RefRO<DistanceBeforeHostileComponent> m_hostileDistanceSq;
		readonly RefRO<AiTelemetry>                    m_telemetry;
	}
}