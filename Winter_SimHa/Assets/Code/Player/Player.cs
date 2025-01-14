using System;
using Code.Animators;
using Code.Core.EventSystems;
using Code.Core.StatSystem;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateListSO playerFSM;

        private StateMachine _stateMachine;

        [SerializeField] private StatSO jumpCountStat, attackSpeedStat;
        [field: SerializeField] public AnimParamSO ComboCounterParam { get; private set; }

        private int _maxJumpCount;
        private int _currentJumpCount;
        public bool CanJump => _currentJumpCount > 0;

        [Header("Temp settings")]
        public Vector2[] atkMovement;
        public Vector2 dashAttackMovement;

        public class SaveGameEvent : GameEvent
        {
            public bool isSaveToFile;
        }

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, playerFSM);



        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            EntityStat statCompo = GetCompo<EntityStat>();
            statCompo.GetStat(jumpCountStat).OnValueChange += HandleJumpCountChange;
            _currentJumpCount = _maxJumpCount = Mathf.RoundToInt(statCompo.GetStat(jumpCountStat).Value);

            PlayerInput.OnDashKeyPressed += HandleDashKeyPress;
            GetCompo<EntityAnimationTrigger>().OnAnimationEnd += HandleAnimationEnd;
        }

        private void OnDestroy()
        {
            GetCompo<EntityStat>().GetStat(jumpCountStat).OnValueChange -= HandleJumpCountChange;
            PlayerInput.OnDashKeyPressed -= HandleDashKeyPress;
            GetCompo<EntityAnimationTrigger>().OnAnimationEnd -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd() => _stateMachine.CurrentState.AnimationEndTrigger();

        private void HandleDashKeyPress()
        {
            //이 부분은 나중에 스킬시스템과 메시징시스템으로 묶는다.
            ChangeState("DASH");
        }

        private void HandleJumpCountChange(StatSO stat, float current, float previous)
            => _maxJumpCount = Mathf.RoundToInt(current);

        public void DecreaseJumpCount() => _currentJumpCount--;
        public void ResetJumpCount() => _currentJumpCount = _maxJumpCount;

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);

    }
}