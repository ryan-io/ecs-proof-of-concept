// src

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace src {
	public readonly partial struct MoveAspect : IAspect {
		public bool IsInHostileRange()
			=> m_telemetry.ValueRO.SqDistanceToPlayer <= m_critDist.ValueRO.DistanceSq;

		public float3 HeadingPlayer => m_telemetry.ValueRO.HeadingToPlayer;

		public float3 HeadingHome => m_telemetry.ValueRO.HeadingToHome;

		public bool IsHome => math.distancesq(
			                      m_telemetry.ValueRO.InitialPosition,
			                      m_transform.ValueRO.Position)
		                      <= m_critDist.ValueRO.DistanceSqFromReturn;

		public void Move(float3 targetPosition, float speed, float deltaTime) {
		}

		public void MoveHome(float speed, float deltaTime) {
			var vectorDelta = speed * HeadingHome * deltaTime;
			m_transform.ValueRW.Position += vectorDelta;
		}

		public void MoveTowardsPlayer(float speed, float deltaTime) {
			var vectorDelta = speed * HeadingPlayer * deltaTime;
			m_transform.ValueRW.Position += vectorDelta;
		}

		public void MoveAwayFromPlayer(float speed, float deltaTime) {
			var vectorDelta = -speed * HeadingPlayer * deltaTime;
			m_transform.ValueRW.Position += vectorDelta;
		}

		readonly EnabledRefRO<MoveableEntityTag> m_moveableEntityTag;
		readonly RefRW<LocalTransform>           m_transform;
		readonly RefRO<CriticalDistances>        m_critDist;
		readonly RefRO<AiTelemetry>              m_telemetry;
	}
}