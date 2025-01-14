using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using System;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerGroundState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;

        protected PlayerGroundState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnJumpKeyPressed += HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed += HandleAttackKeyPress;
        }

        public override void Update()
        {
            base.Update();
            if(_mover.IsGroundDetected() == false && _mover.CanManualMove)
            {
                _player.ChangeState("FALL");
            }
        }

        public override void Exit()
        {
            _player.PlayerInput.OnJumpKeyPressed -= HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed -= HandleAttackKeyPress;
            base.Exit();
        }


        protected virtual void HandleAttackKeyPress()
        {
            if (_mover.IsGroundDetected())
                _player.ChangeState("ATTACK");
        }

        private void HandleJumpKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("JUMP");
        }
    }
}

