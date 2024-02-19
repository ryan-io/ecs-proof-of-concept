﻿using src.systems;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace src {
	public class RandomNumGenAuthoring : MonoBehaviour {
		public class RandomNumGenAuthoringBaker : Baker<RandomNumGenAuthoring> {
			public override void Bake(RandomNumGenAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new RandomNumGenData {
					Value = Random.CreateFromIndex((uint)System.DateTime.Now.Millisecond)
				});
			}
		}
	}
}