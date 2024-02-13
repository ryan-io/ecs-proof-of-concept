using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace src.systems {
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
	public partial class InputListenerSystem : SystemBase {
		protected override void OnCreate() {
			RequireForUpdate<InputData>();
			RequireForUpdate<PlayerData>();

			m_input = new ControlInput();
		}

		protected override void OnUpdate() {
			var directionalValues = m_input.CoreMap.Movement.ReadValue<Vector2>();
			
			
			
			var data   = new InputData { Directional = directionalValues };
			SystemAPI.SetSingleton(data);
		}

		protected override void OnStartRunning() {
			m_player = SystemAPI.GetSingletonEntity<PlayerData>();
			m_input.Enable();

			m_input.CoreMap.WeakAttack.performed   += OnWeakAttackPerformed;
			m_input.CoreMap.StrongAttack.performed += OnStrongAttackPerformed;
			m_input.CoreMap.Dodge.performed        += OnDodgePerformed;
		}  

		protected override void OnStopRunning() {
			m_input.Disable();
			
			m_input.CoreMap.WeakAttack.performed   -= OnWeakAttackPerformed;
			m_input.CoreMap.StrongAttack.performed -= OnStrongAttackPerformed;
			m_input.CoreMap.Dodge.performed        -= OnDodgePerformed;
		}

		void OnWeakAttackPerformed(InputAction.CallbackContext ctx) {
			SystemAPI.SetComponentEnabled<PlayerWeakAttackInputTag>(m_player, true);
		}
		
		void OnStrongAttackPerformed(InputAction.CallbackContext ctx) {
			SystemAPI.SetComponentEnabled<PlayerStrongAttackInputTag>(m_player, true);
		}
		
		void OnDodgePerformed(InputAction.CallbackContext ctx) {
			SystemAPI.SetComponentEnabled<PlayerDodgeInputTag>(m_player, true);
		}
		
		ControlInput m_input;
		Entity      m_player;
	}
}