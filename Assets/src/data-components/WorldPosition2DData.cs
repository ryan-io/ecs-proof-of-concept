using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct WorldPosition2DData : IComponentData {
		public float2 Value;

		public WorldPosition2DData(in float3 pos) => Value = pos.xy;
	}
}