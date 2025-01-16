using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;

namespace Code.Players.States
{
    public abstract class PlayerGroundState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;
        
        public PlayerGroundState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnJumpKeyPressed += HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed += HandleAttackKeyPress;
            _player.PlayerInput.OnCounterKeyPressed += HandleCounterKeyPress;
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGroundDetected() == false && _mover.CanManualMove)
            {
                _player.ChangeState("FALL");
            }
        }

        public override void Exit()
        {
            _player.PlayerInput.OnJumpKeyPressed -= HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed -= HandleAttackKeyPress;
            _player.PlayerInput.OnCounterKeyPressed -= HandleCounterKeyPress;
            base.Exit();
        }

        private void HandleCounterKeyPress()
        {
            //나중에 쿨타임도 체크해야한다.
            _player.ChangeState("COUNTER_ATTACK");
        }

        protected virtual void HandleAttackKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("ATTACK");
        }

        private void HandleJumpKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("JUMP");
        }
    }
}
