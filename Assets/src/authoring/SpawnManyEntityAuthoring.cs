using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace src {
	public class SpawnManyEntityAuthoring : MonoBehaviour {
		[field: SerializeField] public GameObject Prefab                  { get; private set; }
		[field: SerializeField] public int        SpawnCount              { get; private set; } = 1;
		[field: SerializeField] public float      SeparationRadius        { get; private set; } = 0.5f;
		[field: SerializeField] public int        IterationsBeforeDiscard { get; private set; }
		[field: SerializeField] public float      RegionWidth             { get; private set; } = 1.0f;
		[field: SerializeField] public float      RegionHeight            { get; private set; } = 1.0f;

		public class SpawnManyEntityAuthoringBaker : Baker<SpawnManyEntityAuthoring> {
			public override void Bake(SpawnManyEntityAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				var width  = math.abs(authoring.RegionWidth);
				var height = math.abs(authoring.RegionHeight);
  
				AddComponent(entity, new SpawnManyEntityComponent {  
					Count                   = authoring.SpawnCount,
					Entity                  = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
					IterationsBeforeDiscard = authoring.IterationsBeforeDiscard,
					SeparationRadius        = authoring.SeparationRadius,
					RegionWidth             = width,
					RegionHeight            = height
				});

				AddComponent(entity, new AiSpawnComponent());
				//SetComponentEnabled<SpawnManyEntityComponent>(entity, false);
			}
		}
	}
}