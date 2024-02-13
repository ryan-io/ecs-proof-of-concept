// // Assembly-CSharp
//
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Jobs;
// using Vector3 = System.Numerics.Vector3;
//
// namespace src {
// 	[BurstCompile]
// 	public struct SimpleJobImplementation : IJob {
// 		public NativeArray<Vector3> Positions;
// 		public NativeArray<Vector3> Directions;
//   
// 		public void Execute() {
// 			var r = new Unity.Mathematics.Random(1);
// 			for (var i = 0; i < Positions.Length; i++) {
// 				var scalar = r.NextFloat(1, 10);
// 				Positions[i] += 0.001f * scalar * Directions[i];
// 			}
// 		}
//
// 		public SimpleJobImplementation(
// 			NativeArray<Vector3> positions,
// 			NativeArray<Vector3> directions) {
// 			Positions    = positions;
// 			Directions   = directions;
// 		}
// 	}
// }