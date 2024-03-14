// src

using Unity.Entities;
using Unity.Mathematics;

namespace src {
	public struct AiTelemetry : IComponentData {
		public float3 InitialPosition;
		public float3 HeadingToPlayer;
		public float3 HeadingToHome;
		public float  SqDistanceToPlayer;
		public float  SqDistanceToHome;
	}
}