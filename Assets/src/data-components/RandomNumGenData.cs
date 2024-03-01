// src

using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct RandomNumGenData : IComponentData {
		public Random Value;
	}
}