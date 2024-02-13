using Unity.Mathematics;
using UnityEngine;

namespace src.common {
	public static class Extensions {
		public static float2 GetSize(this Sprite sprite) => new(sprite.bounds.size.x, sprite.bounds.size.y);
	}
}