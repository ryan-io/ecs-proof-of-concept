using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace src {
	public class AiAuthoring : MonoBehaviour {
		[field: SerializeField] public bool CanMove { get; private set; } = true;

		[field: SerializeField, ShowIf("@CanMove")]
		public bool CanWalk { get; private set; } = true;

		[field: SerializeField, ShowIf("@CanWalkInternal")]
		public float WalkSpeed { get; private set; } = 1.0f;

		[field: SerializeField, ShowIf("@CanMove")]
		public bool CanRun { get; private set; } = true;

		[field: SerializeField, ShowIf("@CanRunInternal")]
		public float RunSpeed { get; private set; } = 2.0f;

		[field: SerializeField, ShowIf("@CanRunInternal")]
		public float RunStoppingDistance { get; private set; } = 0.25f;

		[field: SerializeField, ShowIf("@CanMove")]
		public bool CanJetpack { get; private set; } = true;

		[field: SerializeField, ShowIf("@CanJetpackInternal")]
		public float JetpackThrustMax { get; private set; } = 1.0f;

		[field: SerializeField, ShowIf("@CanJetpackInternal")]
		public float JetpackLiftMax { get; private set; } = 1.0f;

		[field: SerializeField, ShowIf("@CanJetpackInternal")]
		public float JetpackHoverHeight { get; private set; } = 1.0f;

		[field: SerializeField] public bool IsHostile { get; private set; } = true;

		[field: SerializeField, ShowIf("@IsHostile")]
		public float DistanceBeforeHostileSq { get; private set; } = 100;

		bool CanWalkInternal    => CanMove && CanWalk;
		bool CanRunInternal     => CanMove && CanRun;
		bool CanJetpackInternal => CanMove && CanJetpack;

		public class AiAuthoringBaker : Baker<AiAuthoring> {
			public override void Bake(AiAuthoring authoring) {
				var entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent(entity, new AiTelemetry());

				if (authoring.CanMove) {
					AddComponent<MoveableEntityTag>(entity);
					SetComponentEnabled<MoveableEntityTag>(entity, true);

					if (authoring.CanWalk) {
						AddComponent(entity, new WalkParametersComponent {
							Speed = authoring.WalkSpeed
						});
						SetComponentEnabled<WalkParametersComponent>(entity, true);
					}

					if (authoring.CanRun) {
						AddComponent(entity, new RunParametersComponent {
							Speed            = authoring.RunSpeed,
							StoppingDistance = authoring.RunStoppingDistance
						});
						SetComponentEnabled<RunParametersComponent>(entity, false);
					}

					if (authoring.CanJetpack) {
						AddComponent(entity, new JetpackParametersComponent {
							ThrustMax   = authoring.JetpackThrustMax,
							LiftMax     = authoring.JetpackLiftMax,
							HoverHeight = authoring.JetpackHoverHeight
						});
						SetComponentEnabled<JetpackParametersComponent>(entity, false);
					}
				}

				if (authoring.IsHostile) {
					AddComponent(entity, new DistanceBeforeHostileComponent {
						DistanceSq = authoring.DistanceBeforeHostileSq
					});
				}
			}
		}
	}
}