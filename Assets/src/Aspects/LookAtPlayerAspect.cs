// src

using NSprites;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace src {
	public readonly partial struct LookAtPlayerAspect : IAspect {
		public void SetFlip(int2 value) => m_flip.ValueRW.Value = value;

		public float3 LocalToWorldPosition => m_localToWorld.ValueRO.Position;

		readonly RefRW<Flip>            m_flip;
		readonly RefRO<LocalToWorld>    m_localToWorld;
		readonly RefRO<LookAtPlayerTag> m_lookAt;
	}
}