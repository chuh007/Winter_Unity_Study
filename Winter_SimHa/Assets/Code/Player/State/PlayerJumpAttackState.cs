using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerJumpAttackState : EntityState
    {
        private Player _player;
        private EntityMover _mover;

        public PlayerJumpAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(true);
            _mover.SetGravityScale(0.1f);
            _mover.CanManualMove = false;
            float forwardForce = 2f * _renderer.FacingDirection; // юс╫ц
            Vector2 force = new Vector2(forwardForce, 0);

            _mover.AddForceToEntity(force);
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _player.ChangeState("FALL");
        }

        public override void Exit()
        {
            _mover.CanManualMove = true;
            _mover.SetGravityScale(1f);
            base.Exit();
        }
    }
}

