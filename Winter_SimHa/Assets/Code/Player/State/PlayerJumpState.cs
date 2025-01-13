using Code.Animators;
using Code.Entities;
using System;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerJumpState : PlayerAirState
    {
        public PlayerJumpState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.Jump();
            _mover.OnMove.AddListener(HandleVelocityChange);
        }

        public override void Exit()
        {
            base.Exit();
            _mover.OnMove.RemoveListener(HandleVelocityChange);
        }

        private void HandleVelocityChange(Vector2 velocity)
        {
            if (velocity.y < 0)
                _player.ChangeState("FALL");
        }
    }
}

