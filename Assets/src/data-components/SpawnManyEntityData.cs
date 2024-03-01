// src

using Unity.Entities;

namespace src {
	public struct SpawnManyEntityData : IComponentData {
		public Entity Entity;
		public int    Count;
		public int    IterationsBeforeDiscard;
		public float  SeparationRadius;
		public float  RegionWidth;
		public float  RegionHeight;
	}
}