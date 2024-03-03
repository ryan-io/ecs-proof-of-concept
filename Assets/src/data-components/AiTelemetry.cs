// src

using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct AiTelemetry : IComponentData {
		public float  SqDistanceToPlayer;
		public float3 HeadingToPlayer;
	}
}