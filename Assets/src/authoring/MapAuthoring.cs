using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace src {
	public class MapAuthoring : MonoBehaviour {
		[field: SerializeField, Required] public GameObject TerrainPrefab { get; private set; }

		[field: SerializeField, OnValueChanged("ValidateLengthAndHeight")]
		public int GridWidth { get; private set; }

		[field: SerializeField, OnValueChanged("ValidateLengthAndHeight")]
		public int GridHeight { get; private set; }

		public class MapAuthoringBaker : Baker<MapAuthoring> {
			public override void Bake(MapAuthoring authoring) {
				var e       = GetEntity(TransformUsageFlags.None);
				var terrain = GetEntity(authoring.TerrainPrefab, TransformUsageFlags.None);
				
				AddComponent(e, new MapComponent {
					Terrain =  terrain,
					Width = authoring.GridWidth,
					Height = authoring.GridHeight
				});
			}
		}

		void ValidateLengthAndHeight() {
			if (GridWidth <= 1)
				GridWidth = 2;

			if (GridHeight <= 1)
				GridHeight = 2;

			if (GridWidth % 2 != 0)
				GridWidth++;

			if (GridHeight % 2 != 0)
				GridHeight++;
		}
	}
}