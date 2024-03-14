using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace src {
	public class RandomNumGenAuthoring : MonoBehaviour {
		public class RandomNumGenAuthoringBaker : Baker<RandomNumGenAuthoring> {
			public override void Bake(RandomNumGenAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.None);
				AddBuffer<Position2DBuffer>(entity);
				
				AddComponent(entity, new RandomNumGenData {
					Value = Random.CreateFromIndex((uint)System.DateTime.Now.Millisecond)
				});
			}
		}
	}
}