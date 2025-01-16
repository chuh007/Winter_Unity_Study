using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Players
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action OnJumpKeyPressed;
        public event Action OnAttackKeyPressed;
        public event Action OnDashKeyPressed;
        public event Action OnSlideKeyPressed;
        public event Action OnCounterKeyPressed;
        
        public Vector2 InputDirection { get; private set; }
        
        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            InputDirection = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackKeyPressed?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnJumpKeyPressed?.Invoke();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnDashKeyPressed?.Invoke();
        }

        public void OnSlide(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSlideKeyPressed?.Invoke();
        }

        public void OnCounter(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnCounterKeyPressed?.Invoke();
        }
    }
}