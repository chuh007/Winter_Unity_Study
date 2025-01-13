using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerAirState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;

        public PlayerAirState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.SetMoveSpeedMultiplier(0.7f);
        }
        public override void Update()
        {
            float xInput = _player.PlayerInput.InputDirection.x;
            if(Mathf.Abs(xInput) > 0)
                _mover.SetMovementX(xInput);
        }
        public override void Exit()
        {
            _mover.SetMoveSpeedMultiplier(1f);
            base.Exit();
        }
    }
}

