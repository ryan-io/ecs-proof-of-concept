// src

using Unity.Entities;

namespace src {
	public struct CriticalDistances : IComponentData {
		public float DistanceSq;
		public float DistanceSqFromReturn;
	}
}