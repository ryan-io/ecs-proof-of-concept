using Unity.Entities;
using UnityEngine;

namespace src {
	public class InputAuthoring : MonoBehaviour {
		public class InputAuthoringBaker : Baker<InputAuthoring> {
			public override void Bake(InputAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
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