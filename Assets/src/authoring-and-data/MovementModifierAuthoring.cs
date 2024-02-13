// Assembly-CSharp

using Unity.Entities;
using UnityEngine;

namespace src {
	public struct MovementModifierData : IComponentData {
		public float Speed;
	}
	
	public class MovementModifierAuthoring : MonoBehaviour {
		[field: SerializeField] public float Penalty { get; private set; }

		public class MovementModifierAuthoringBaker : Baker<MovementModifierAuthoring> {
			public override void Bake(MovementModifierAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new MovementModifierData { Speed = authoring.Penalty });
			}
		}
	}
}