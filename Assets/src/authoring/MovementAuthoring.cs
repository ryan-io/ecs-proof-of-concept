using Unity.Entities;
using UnityEngine;

namespace src {
	public class MovementAuthoring : MonoBehaviour {
		[field: SerializeField] public float PenaltyMultiplier { get; private set; } = 1.0f;

		public class MovementModifierAuthoringBaker : Baker<MovementAuthoring> {
			public override void Bake(MovementAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new MovementModifierComponent { Speed = authoring.PenaltyMultiplier });
				AddComponent(entity, new MovementStatusComponent());
				SetComponentEnabled<MovementStatusComponent>(entity, true);
			}
		}
	}
}