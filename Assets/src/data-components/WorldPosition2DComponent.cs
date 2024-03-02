using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct WorldPosition2DComponent : IComponentData {
		public float2 Value;

		public WorldPosition2DComponent(in float3 pos) => Value = pos.xy;
	}
}