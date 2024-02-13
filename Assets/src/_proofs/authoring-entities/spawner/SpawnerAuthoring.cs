// using Unity.Entities;
// using UnityEngine;
//
// namespace src {
// 	public struct SpawnerComponentData : IComponentData {
// 		public Entity Prefab;
// 		public int    SpawnsOnTick;
// 		public float  SpawnDelay;
// 		public float  NextSpawnTime;
// 	}
//
// 	public class SpawnerAuthoring : MonoBehaviour {
// 		[field: SerializeField] public GameObject Prefab            { get; private set; }
// 		[field: SerializeField] public int        InitialSpawnCount { get; private set; }
// 		[field: SerializeField] public float      SpawnDelay        { get; private set; }
//
// 		class SpawnerBaker : Baker<SpawnerAuthoring> {
// 			public override void Bake(SpawnerAuthoring authoring) {
// 				var prefabEntity = GetEntity(TransformUsageFlags.Dynamic);
//
// 				AddComponent(prefabEntity, new SpawnerComponentData {
// 					Prefab       = GetEntity(authoring.Prefab, TransformUsageFlags.None),
// 					SpawnsOnTick = authoring.InitialSpawnCount,
// 					SpawnDelay =  authoring.SpawnDelay,
// 					NextSpawnTime = 0.0f
// 				});
// 			}
// 		}
// 	}
// }