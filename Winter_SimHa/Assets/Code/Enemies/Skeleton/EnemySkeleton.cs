using Code.Combats;
using Code.Enemies.BTCommons;
using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemySkeleton : BTEnemy, ICounterable
    {
        private StateChangeEvent _stateChannel;
        private BlackboardVariable<BTEnemyState> _state;
        private BlackboardVariable<float> _stunTume;

        private EntityMover _mover;
        private EntityFeedbackData _feedbackData;
        private EntityAnimationTrigger _animationTrigger;
        private EnemyAttackCompo _attackCompo;
        private EntityHealth _healthCompo;

        #region Initialize section

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover = GetCompo<EntityMover>();
            _feedbackData = GetCompo<EntityFeedbackData>();
            GetCompo<EntityHealth>().OnKnockback += HandleKnockBack;
            _animationTrigger = GetCompo<EntityAnimationTrigger>();
            _animationTrigger.OnCounterStatuschange += SetCounterStatus;
            _attackCompo = GetCompo<EnemyAttackCompo>();
            _healthCompo = GetCompo<EntityHealth>();
        }

        // BT의 인스턴스 생성은 Awake에서 이루어지기 때문에 반드시 Start에서 가져오는 작업을 해야한다.
        protected override void Start()
        {
            BlackboardVariable<StateChangeEvent> stateChannelVariable =
                GetBlackboardVariable<StateChangeEvent>("StateChannel");
            _stateChannel = stateChannelVariable.Value;
            Debug.Assert(_stateChannel != null, $"StateChannel variable is null {gameObject.name}");

            _state = GetBlackboardVariable<BTEnemyState>("EnemyState");
            _stunTume = GetBlackboardVariable<float>("StunTime");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCompo<EntityHealth>().OnKnockback -= HandleKnockBack;
            _animationTrigger.OnCounterStatuschange -= SetCounterStatus;

        }

        #endregion

        private void HandleKnockBack(Vector2 knockBackForce)
        {
            float knockBackTime = 0.5f;
            _mover.KnockBack(knockBackForce, knockBackTime);
        }
        protected override void HandleHit()
        {
            if (IsDead) return;

            // 스턴상태에서는 무시
            if (_state.Value == BTEnemyState.STUN || _state.Value == BTEnemyState.HIT) return;

            if (_feedbackData.IsLastHitPowerAttack)
            {
                _stateChannel.SendEventMessage(BTEnemyState.HIT);
            }
            else if (_state.Value == BTEnemyState.PATROL)
            {
                _stateChannel.SendEventMessage(BTEnemyState.CHASE);
            }
        }

        protected override void HandleDead()
        {
            if(IsDead) return;
            gameObject.layer = DeadBodyLayer;
            IsDead = true;
            _stateChannel.SendEventMessage(BTEnemyState.DEATH);
        }

        #region

        public bool CanCounter { get; private set; }
        public Transform TargetTrm { get; }
        public void ApplyCounter(float damage, Vector2 direction, Vector2 knockBackForce, bool isPowerAttack, Entity dealer)
        {
            float stunTime = 2f;

            CanCounter = false;
            _stunTume.Value = stunTime;
            _stateChannel.SendEventMessage(BTEnemyState.STUN);
            _healthCompo.ApplyDamage(damage, direction, knockBackForce, isPowerAttack, dealer);
        }

        private void SetCounterStatus(bool canCounter)
            =>CanCounter = canCounter;

        #endregion
    }

}
