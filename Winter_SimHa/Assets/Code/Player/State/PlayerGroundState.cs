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
            _player.PlayerInput.OnJumpKeyPressed += HandleHumpKeyPress;
        }

        public override void Update()
        {
            base.Update();
            if(_mover.IsGroundDetected() == false)
            {
                _player.ChangeState("FALL");
            }
        }

        public override void Exit()
        {
            _player.PlayerInput.OnJumpKeyPressed -= HandleHumpKeyPress;
            base.Exit();
        }

        private void HandleHumpKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("JUMP");
        }
    }
}

