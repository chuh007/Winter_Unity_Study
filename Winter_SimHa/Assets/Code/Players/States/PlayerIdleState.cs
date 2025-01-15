using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(false);
        }

        public override void Update()
        {
            base.Update();

            float xInput = _player.PlayerInput.InputDirection.x;

            if (Mathf.Abs(xInput) > 0)
            {
                _player.ChangeState("MOVE");
            }
        }
    }
}