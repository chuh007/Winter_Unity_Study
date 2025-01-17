using System;
using Code.Animators;
using Code.Core.EventSystems;
using Code.Core.StatSystem;
using Code.Entities;
using Code.Entities.FSM;
using Code.SkillSystem;
using Code.SkillSystem.Dash;
using UnityEngine;

namespace Code.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateListSO playerFSM;

        private StateMachine _stateMachine;

        [SerializeField] private StatSO jumpCountStat, attackSpeedStat;
        [field: SerializeField] public AnimParamSO ComboCounterParam { get; private set; }
        
        private int _maxJumpCount;
        private int _currentJumpCount;
        public bool CanJump => _currentJumpCount > 0;
        
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
            // ^-^ 점프카운트 리셋 ^-^
            _currentJumpCount = _maxJumpCount = Mathf.RoundToInt(statCompo.GetStat(jumpCountStat).Value);

            PlayerInput.OnDashKeyPressed += HandleDashKeyPress;
            GetCompo<EntityAnimationTrigger>().OnAnimationEnd += HandleAnimationEnd;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCompo<EntityStat>().GetStat(jumpCountStat).OnValueChange -= HandleJumpCountChange;
            PlayerInput.OnDashKeyPressed -= HandleDashKeyPress; //필요 없긴 하다.
            GetCompo<EntityAnimationTrigger>().OnAnimationEnd -= HandleAnimationEnd;

            PlayerInput.ClearSubscription();
        }

        protected override void HandleHit()
        {
            //피격상태로 전환
        }

        protected override void HandleDead()
        {
            //죽음상태로 전환해주면 된다.
        }

        private void HandleAnimationEnd() => _stateMachine.CurrentState.AnimationEndTrigger();

        private void HandleDashKeyPress()
        {
            //이 부분은 나중에 메시징시스템으로 묶는다.
            float facingDirection = GetCompo<EntityRenderer>().FacingDirection;
            if (GetCompo<EntityMover>().IsWallDetected(facingDirection)) return;
            
            if(GetCompo<SkillCompo>().GetSkill<DashSkill>().AttemptUseSkill())
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