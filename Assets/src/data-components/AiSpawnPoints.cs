using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct AiSpawnPoints : IComponentData {
		public DynamicBuffer<float3> Positions;
	}
}