// src

using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct InputComponent : IComponentData {
		public float2 Directional;
	}
}