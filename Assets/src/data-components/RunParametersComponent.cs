// src

using Unity.Entities;

namespace src {
	public struct RunParametersComponent : IComponentData, IEnableableComponent {
		public float Speed;
		public float StoppingDistance;
	}
}