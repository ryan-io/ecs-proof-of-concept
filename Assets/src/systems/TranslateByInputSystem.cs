using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace src.systems {
	[BurstCompile]
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial struct TranslateByInputSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<InputData>();
			state.RequireForUpdate<MovementModifierData>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var dT = SystemAPI.Time.DeltaTime;

			foreach (var translate in SystemAPI.Query<TranslateAspect>()) {
				translate.Translate(dT);
			}
		}
	}

	public readonly partial struct TranslateAspect : IAspect {
		public void Translate(float dT) {
			var dir = m_input.ValueRO.Directional;
			m_transform.ValueRW.Position.xy += dir * dT * m_movement.ValueRO.Speed;
		}

		readonly RefRO<InputData>            m_input;
		readonly RefRO<MovementModifierData> m_movement;
		readonly RefRW<LocalTransform>       m_transform;
	}
}