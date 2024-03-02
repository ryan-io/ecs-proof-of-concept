// src

using Unity.Entities;

namespace src {
	public struct WeaponComponent : IComponentData {
		public float DamageMin;
		public float DamageMax;
		public float Speed;
	}
}