// src

using Unity.Entities;
using Unity.Mathematics;

namespace src.systems {
	public struct RandomNumGenData : IComponentData {
		public Random Value;
	}
}