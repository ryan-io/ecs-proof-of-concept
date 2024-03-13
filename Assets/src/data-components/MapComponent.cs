// src

using Unity.Entities;

namespace src {
	public struct MapComponent : IComponentData {
		public       Entity Terrain;
		public const float  TILE_SIZE = 0.5f;
		public       int    Width;
		public       int    Height;
	}
}