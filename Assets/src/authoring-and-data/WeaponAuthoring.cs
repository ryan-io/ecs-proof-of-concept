// Assembly-CSharp

using Unity.Entities;
using UnityEngine;

namespace src {
	public struct WeaponData : IComponentData {
		public float DamageMin;
		public float DamageMax;
		public float Speed;
	}

	public class WeaponAuthoring : MonoBehaviour {
		[field: SerializeField] public float DamageMin       { get; private set; } = 1.0f;
		[field: SerializeField] public float DamageMax       { get; private set; } = 10f;
		[field: SerializeField] public float Speed           { get; private set; } = 2.0f;
		[field: SerializeField] public float MovementPenalty { get; private set; } = 1.0f;

		public class WeaponAuthoringBaker : Baker<WeaponAuthoring> {
			public override void Bake(WeaponAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity,
					new WeaponData {
						DamageMin = authoring.DamageMin,
						DamageMax = authoring.DamageMax,
						Speed     = authoring.Speed,
					});

				AddComponent(entity, new MovementModifierData() {
					Speed = authoring.MovementPenalty
				});
			}
		}
	}
}