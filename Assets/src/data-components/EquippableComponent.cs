using Unity.Entities;

namespace src {
	public struct EquippableComponent : IComponentData {
		public Entity Prefab;
	}
}