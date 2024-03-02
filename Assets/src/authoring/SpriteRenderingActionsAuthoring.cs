// src

using Unity.Entities;
using UnityEngine;

namespace src {
	public class SpriteRenderingActionsAuthoring : MonoBehaviour {
		[field: SerializeField] public bool ShouldLookAtPlayer { get; private set; } = true;
		
		public class SpriteRenderingActionsAuthoringBaker : Baker<SpriteRenderingActionsAuthoring> {
			public override void Bake(SpriteRenderingActionsAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);

				if (authoring.ShouldLookAtPlayer) {
					AddComponent(entity, new LookAtPlayerTag());
				}
			}
		}
	}
}