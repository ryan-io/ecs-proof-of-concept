using Unity.Entities;

namespace src {
	public struct EquippableData : IComponentData {
		public Entity Prefab;
	}
}