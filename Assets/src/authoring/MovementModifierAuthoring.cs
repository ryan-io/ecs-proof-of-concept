using Unity.Entities;
using UnityEngine;

namespace src {
	public class MovementModifierAuthoring : MonoBehaviour {
		[field: SerializeField] public float Penalty { get; private set; }

		public class MovementModifierAuthoringBaker : Baker<MovementModifierAuthoring> {
			public override void Bake(MovementModifierAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new MovementModifierComponent { Speed = authoring.Penalty });
			}
		}
	}
}