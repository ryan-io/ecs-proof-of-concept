using src.systems;
using Unity.Entities;
using UnityEngine;

namespace src {
	public class Authoring : MonoBehaviour {
		[field: SerializeField] public GameObject Prefab     { get; private set; }
		[field: SerializeField] public int        SpawnCount { get; private set; }
		
		public class AuthoringBaker : Baker<Authoring> {

			public override void Bake(Authoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new SpawnEntityData {
					Count = authoring.SpawnCount,
					Entity = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
				});
			}
		}
	}
}