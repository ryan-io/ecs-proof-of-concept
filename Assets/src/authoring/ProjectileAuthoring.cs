﻿using Unity.Entities;
using UnityEngine;

namespace src {
	public class ProjectileAuthoring : MonoBehaviour {
		[field: SerializeField] public GameObject Prefab { get; private set; }
		
		public class ProjectileAuthoringBaker : Baker<ProjectileAuthoring> {
			public override void Bake(ProjectileAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new ProjectileComponent {
					Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
				});
			}
		}
	}
}