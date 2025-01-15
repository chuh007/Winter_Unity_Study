using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerDashAttackState :EntityState
    {
        private Player _player;
        private EntityMover _mover;
        
        public PlayerDashAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = false;
            Vector2 movement = _player.dashAttackMovement; //이따 만들께
            movement.x *= _renderer.FacingDirection;
            
            _mover.AddForceToEntity(movement);
        }

        public override void Update()
        {
            base.Update();
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _mover.CanManualMove = true;
            base.Exit();
        }
    }
}