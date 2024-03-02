using Sirenix.OdinInspector;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace src {
	public class WeaponAttachmentAuthoring : MonoBehaviour {
		[field: SerializeField, Required] public GameObject Pivot { get; private set; }

		[field: SerializeField] public float3 Offset { get; private set; }

		public class WeaponAttachmentAuthoringBaker : Baker<WeaponAttachmentAuthoring> {
			public override void Bake(WeaponAttachmentAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity,
					new WeaponAttachmentComponent {
						Pivot  = GetEntity(authoring.Pivot, TransformUsageFlags.Dynamic), 
						Offset = authoring.Offset
					});
			}
		}
	}
}