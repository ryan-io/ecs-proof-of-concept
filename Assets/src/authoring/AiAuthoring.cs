using Unity.Entities;
using UnityEngine;

namespace src {
	public class AiAuthoring : MonoBehaviour {
		public class LookAtPlayerAuthoringBaker : Baker<AiAuthoring> {
			public override void Bake(AiAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
			}
		}
	}
}