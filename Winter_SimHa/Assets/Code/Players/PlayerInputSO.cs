﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Players
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
    {
        public event Action OnJumpKeyPressed;
        public event Action OnAttackKeyPressed;
        public event Action OnDashKeyPressed;
        public event Action OnSlideKeyPressed;
        public event Action OnCounterKeyPressed;
        public event Action<bool> OnSkillKeyPressed;
        public event Action OnOpenMenuKeyPressed;
        public event Action<int> OnMenuSlideEvent;
        public event Action<Vector2> OnUINavigateEvent;
        public event Action OnUIInteractEvent;
        public event Action OnUICancelEvent;
        public event Action OnUISubmitEvent;
        
        public Vector2 InputDirection { get; private set; }
        
        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                _controls.UI.SetCallbacks(this);
            }
            _controls.Player.Enable();
            _controls.UI.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.UI.Disable();
        }

        public void ClearSubscription()
        {
            OnJumpKeyPressed = null;
            OnAttackKeyPressed = null;
            OnDashKeyPressed = null;
            OnSlideKeyPressed = null;
            OnCounterKeyPressed = null;
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

        public void OnSkill(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSkillKeyPressed?.Invoke(true);
            if(context.canceled)
                OnSkillKeyPressed?.Invoke(false);
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 uiMovement = context.ReadValue<Vector2>();
                OnUINavigateEvent?.Invoke(uiMovement);
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnUISubmitEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            
        }


        public void OnOpenMenu(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnOpenMenuKeyPressed?.Invoke();
        }

        public void OnMenuSlideLeft(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnMenuSlideEvent?.Invoke(-1);
        }

        public void OnMenuSlideRight(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnMenuSlideEvent?.Invoke(1);
        }

        public void OnUIInteract(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnUIInteractEvent?.Invoke();
        }

        public void OnUICancel(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnUICancelEvent?.Invoke();
        }

        public void SetPlayerInput(bool isActive)
        {
            if(isActive)
                _controls.Player.Enable();
            else
                _controls.Player.Disable();
        }

        #region UnUsedEvent
        public void OnPoint(InputAction.CallbackContext context){}
        public void OnClick(InputAction.CallbackContext context){}
        public void OnRightClick(InputAction.CallbackContext context){}
        public void OnMiddleClick(InputAction.CallbackContext context){}
        public void OnScrollWheel(InputAction.CallbackContext context){}
        public void OnTrackedDevicePosition(InputAction.CallbackContext context){}
        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context){}
        #endregion

    }
}