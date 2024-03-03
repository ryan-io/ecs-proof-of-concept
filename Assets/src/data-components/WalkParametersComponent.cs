// src

using Unity.Entities;

namespace src {
	public struct WalkParametersComponent : IComponentData, IEnableableComponent {
		public float Speed;
	}
}