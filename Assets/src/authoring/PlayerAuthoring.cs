using Unity.Entities;
using UnityEngine;

namespace src {
	public class PlayerAuthoring : MonoBehaviour {
		[field: SerializeField] public float InitialMovementSpeed { get; private set; } = 1.0f;
		[field: SerializeField] public float InitialHealth { get; private set; } = 10.0f;
		
		public class PlayerAuthoringBaker : Baker<PlayerAuthoring> {
			public override void Bake(PlayerAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity, new PlayerComponent {
					Entity =  entity,
				//	Health = authoring.InitialHealth
				});
				
				AddComponent(entity, new MovementModifierComponent {Speed = authoring.InitialMovementSpeed} );
				
				AddComponent(entity, new InputComponent());
				
				AddComponent(entity, new PlayerWeakAttackInputTag());
				SetComponentEnabled<PlayerWeakAttackInputTag>(entity, false);
				
				AddComponent(entity, new PlayerStrongAttackInputTag());
				SetComponentEnabled<PlayerStrongAttackInputTag>(entity, false);
				
				AddComponent(entity, new PlayerDodgeInputTag());
				SetComponentEnabled<PlayerDodgeInputTag>(entity, false);
			}
		}
	}
}