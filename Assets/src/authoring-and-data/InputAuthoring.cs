using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace src {
	public struct InputData : IComponentData {
		public float2 Directional;
	}
	
	public struct PlayerWeakAttackInputTag : IComponentData, IEnableableComponent {
		public int TestInt;
	}

	public struct PlayerStrongAttackInputTag : IComponentData, IEnableableComponent {
		
	}
	
	public struct PlayerDodgeInputTag : IComponentData, IEnableableComponent {
		
	}
    
	public class InputAuthoring : MonoBehaviour {
		public class InputAuthoringBaker : Baker<InputAuthoring> {
			public override void Bake(InputAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new InputData());
				
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