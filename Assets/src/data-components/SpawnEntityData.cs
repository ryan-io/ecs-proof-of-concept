// src

using Unity.Entities;

namespace src.systems {
	public struct SpawnEntityData : IComponentData {
		public Entity Entity;
		public int    Count;
	}
}