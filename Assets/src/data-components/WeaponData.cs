// src

using Unity.Entities;

namespace src {
	public struct WeaponData : IComponentData {
		public float DamageMin;
		public float DamageMax;
		public float Speed;
	}
}