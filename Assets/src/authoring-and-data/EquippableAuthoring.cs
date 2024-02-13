// Assembly-CSharp

using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace src {
	public struct EquippableData : IComponentData {
		public Entity Prefab;
	}
	
	public class EquippableAuthoring : MonoBehaviour {
		[field: SerializeField, Required] public GameObject Prefab { get; private set; }

		public class EquippableAuthoringBaker : Baker<EquippableAuthoring> {
			public override void Bake(EquippableAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity,
					new EquippableData {
						Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
					});
			}
		}
	}
}