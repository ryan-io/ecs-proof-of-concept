// src

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace src.systems {
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class ModifyMainCameraPositionSystem : SystemBase {
		protected override void OnCreate() {
			m_camera = Camera.main;
		}

		protected override void OnUpdate() {
			var player    = SystemAPI.GetSingletonEntity<PlayerComponent>();
			var localTr   = SystemAPI.GetComponent<LocalToWorld>(player);
			var camTr     = m_camera.transform;
			var posVector = new Vector3(localTr.Position.x, localTr.Position.y, camTr.position.z);
			camTr.position = posVector;
		}

		Camera m_camera;
	}
}