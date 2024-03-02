// src

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src.Aspects {
	public readonly partial struct PlayerAspect : IAspect {
		public readonly Entity Self;

		public float3 GetHeadingTo(ref LocalTransform tr) {
			return math.normalizesafe(tr.Position - m_transform.ValueRO.Position);
		}

		readonly RefRO<LocalTransform>  m_transform;
		readonly RefRO<PlayerComponent> m_player;
	}
}