// src

using Unity.Entities;

namespace src {
	public struct DistanceBeforeHostileComponent : IComponentData {
		public float DistanceSq;
	}
}