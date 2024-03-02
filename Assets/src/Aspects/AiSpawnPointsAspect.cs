// src

using Unity.Entities;

namespace src.Aspects {
	public readonly partial struct AiSpawnPointsAspect : IAspect {
		public readonly Entity Self;

		public bool IsSet => m_aiSpawnPoints.ValueRO.Value.IsCreated &&
		                     m_aiSpawnPoints.ValueRO.Value.Value.Positions.Length > 0;

		public void SetPositions(BlobAssetReference<PositionsBlob> blob) {
			m_aiSpawnPoints.ValueRW.Value = blob;
		}
		//public float3 this[int index] => Positions[index];

		//public BlobArray<float3> Positions => m_aiSpawnPoints.ValueRW.Value.Value.Positions;

		readonly RefRW<AiSpawnComponent> m_aiSpawnPoints;
	}
}