// using System.Collections.Generic;
// using Unity.Collections;
// using Unity.Jobs;
// using UnityEngine;
//
// namespace src {
// 	public class Spawner : MonoBehaviour {
// 		[field: SerializeField] int Count { get; set; }
//
// 		[field: SerializeField] GameObject Prefab { get; set; }
//
// 		void Awake() {
// 			_positions  = new NativeArray<System.Numerics.Vector3>(Count, Allocator.Persistent);
// 			_directions = new NativeArray<System.Numerics.Vector3>(Count, Allocator.Persistent);
// 			_transforms = new Transform[Count];
// 		}
//
// 		void Start() {
// 			for (var i = 0; i < Count; i++) {
// 				var iniTr = _transforms[i] = Instantiate(Prefab).transform;
// 				var rDir  = Random.insideUnitCircle;
// 				iniTr.rotation = new Quaternion(rDir.x, rDir.y, 0, 1);
// 				_directions[i] = System.Numerics.Vector3.UnitX;
// 			}
// 		}
//
// 		void Update() {
// 			for (var j = 0; j < _positions.Length; j++) {
// 				var p = _transforms[j].position;
// 				_positions[j] = new System.Numerics.Vector3(p.x, p.y, 0);
// 			}
//
// 			var                     sortJob       = _positions.SortJob(new AxisXComparison());
// 			var                     sortJobHandle = sortJob.Schedule();
// 			SimpleJobImplementation workJob       = new(_positions, _directions);
// 			JobHandle               workJobHandle = workJob.Schedule(sortJobHandle);
// 			workJobHandle.Complete();
//
// 			for (var k = 0; k < _transforms.Length; k++) {
// 				var p = _positions[k];
// 				_transforms[k].position = new Vector3(p.X, p.Y, 0);
// 			}
// 		}
//
// 		void OnDestroy() {
// 			_positions.Dispose();
// 			_directions.Dispose();
// 		}
//
// 		NativeArray<System.Numerics.Vector3> _positions;
// 		NativeArray<System.Numerics.Vector3> _directions;
// 		Transform[]                          _transforms;
// 	}
//
// 	public struct AxisXComparison : IComparer<System.Numerics.Vector3> {
// 		public int Compare(System.Numerics.Vector3 v1, System.Numerics.Vector3 v2) {
// 			if (v1.X > v2.X)
// 				return 1;
//
// 			return v1.X < v2.X ? -1 : 0;
// 		}
// 	}
// }