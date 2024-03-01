using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct PositionsBlob {
		public BlobArray<float3> Positions;
	}
	
	public struct AiSpawnPoints : IComponentData {
		public BlobAssetReference<PositionsBlob> Value;
	}
}