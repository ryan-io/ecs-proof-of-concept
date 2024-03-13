// src

using Unity.Entities;
using Unity.Mathematics;

namespace src {
	[InternalBufferCapacity(16)]
	public struct Position2DBuffer : IBufferElementData {
		public float2 Position;
	}
}