// src

using Unity.Entities;

namespace src {
	public struct JetpackParametersComponent : IComponentData, IEnableableComponent {
		public float ThrustMax;
		public float LiftMax;
		public float HoverHeight;
	}
}