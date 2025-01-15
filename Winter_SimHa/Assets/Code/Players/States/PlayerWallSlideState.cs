using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerWallSlideState : EntityState
    {
        private Player _player;
        private EntityMover _mover;

        private const float WALL_SLIDE_GRAVITY_SCALE = 0.3f;
        private const float WALL_SLIDE_LIMIT_SPEED = 5f;
        private const float NORMAL_LIMIT_SPEED = 40f;
        public PlayerWallSlideState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(true);
            _mover.SetLimitYSpeed(WALL_SLIDE_LIMIT_SPEED);
            _mover.SetGravityScale(WALL_SLIDE_GRAVITY_SCALE);
        }

        public override void Update()
        {
            base.Update();
            float xInput = _player.PlayerInput.InputDirection.x;
            if (Mathf.Abs(xInput + _renderer.FacingDirection) < 0.5f)
            {
                _player.ChangeState("FALL");
                return;
            }
            
            //쭉 내려가다가 땅에 닿았다면 IDLE로 변경해야 한다.
            if (_mover.IsGroundDetected() || _mover.IsWallDetected(_renderer.FacingDirection) == false)
            {
                _player.ChangeState("IDLE");
                _player.ResetJumpCount();
            }
        }

        public override void Exit()
        {
            _mover.SetGravityScale(1f);
            _mover.SetLimitYSpeed(NORMAL_LIMIT_SPEED);
            base.Exit();
        }
    }
}