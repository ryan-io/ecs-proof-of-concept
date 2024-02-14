// src

using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct WeaponAttachmentData : IComponentData {
		public Entity Pivot;
		public float3 Offset;
	}
}