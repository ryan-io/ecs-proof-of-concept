using Unity.Entities;
using UnityEngine;

namespace src {
	public struct PlayerData : IComponentData {
		public int Health;
	}
	
	public class PlayerAuthoring : MonoBehaviour {
		[field: SerializeField] public float InitialMovementSpeed { get; private set; } = 1.0f;
		
		public class PlayerAuthoringBaker : Baker<PlayerAuthoring> {
			public override void Bake(PlayerAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity, new PlayerData());
				AddComponent(entity, new MovementModifierData {Speed = authoring.InitialMovementSpeed} );
			}
		}
	}
}