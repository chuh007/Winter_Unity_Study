using Code.Animators;
using Code.Combats;
using Code.Entities;
using Code.Entities.FSM;
using System;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerAttackState : EntityState
    {
        private Player _player;
        private EntityMover _mover;
        private PlayerAttackCompo _attackCompo;

        private int _comboCounter;
        private float _lastAttackTime;
        private readonly float _comboWindow = 0.8f; //콤보가 이어지도록 하는 시간제한
        private const int MAX_COMBO_COUNT = 2;
        
        public PlayerAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            if (_comboCounter > MAX_COMBO_COUNT || Time.time >= _lastAttackTime + _comboWindow)
                _comboCounter = 0;
            
            _renderer.SetParam(_player.ComboCounterParam, _comboCounter);
            _mover.CanManualMove = false; //움직이지 못하게
            _mover.StopImmediately(true);

            SetAttackData();
        }

        private void SetAttackData()
        {
            float atkDirection = _renderer.FacingDirection;
            float xInput = _player.PlayerInput.InputDirection.x;

            if (Mathf.Abs(xInput) > 0)
                atkDirection = Mathf.Sign(xInput);

            AttackDataSO attackData = _attackCompo.GetAttackData($"PlayerCombo{_comboCounter}");

            Vector2 movement = _player.atkMovement[_comboCounter];
            movement.x *= atkDirection;
            _mover.AddForceToEntity(movement);

            _attackCompo.SetAttackData(attackData);
        }

        public override void Update()
        {
            base.Update();
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            ++_comboCounter;
            _lastAttackTime = Time.time;
            _mover.CanManualMove = true; //이거 잊지마
            base.Exit();
        }
    }
}