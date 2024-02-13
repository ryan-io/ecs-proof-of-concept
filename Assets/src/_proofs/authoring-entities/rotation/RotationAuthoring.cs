// using Unity.Entities;
// using UnityEngine;
//
// namespace src {
// 	public struct RotationData : IComponentData {
// 		public float RotationSpeed;
//
// 		// see example on DependsOn() for external dependencies
// 		public int VertexCount;
// 	}
// 	public class RotationAuthoring : MonoBehaviour {
// 		public Mesh  RotationMesh;
// 		public float RotationSpeed = 10f;
//
// 		class RotationBaker : Baker<RotationAuthoring> {
// 			public override void Bake(RotationAuthoring authoring) {
// 				// we can track external dependencies through the DependOn() method:
// 				DependsOn(authoring.RotationMesh);
//
// 				if (!authoring.RotationMesh)
// 					return;
//
// 				// Before baking
// 				const TransformUsageFlags transformUsageFlags = TransformUsageFlags.Dynamic;
// 				Entity                    entity              = GetEntity(transformUsageFlags);
//
// 				// Create a new instance of component data -> maps data from Monobehavior
// 				RotationData data = new RotationData {
// 					RotationSpeed = authoring.RotationSpeed,
// 					VertexCount   = authoring.RotationMesh.vertexCount
// 				};
//
// 				// Bake the entity
// 				AddComponent(entity, data);
// 			}
// 		}
// 	}
// }