using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerAttackState : EntityState
    {
        private Player _player;
        private EntityMover _mover;

        private int _comboCounter;
        private float _lastAttackTime;
        private readonly float _comboWindow = 0.8f; //콤보가 이어지도록 하는 시간제한
        private const int MAX_COMBO_COUNT = 2;

        public PlayerAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            //최대 콤보에 도달했거나, 최종공격으로부터 콤보 윈도우시간 이상 흘렀다면 콤보 초기화
            if (_comboCounter > MAX_COMBO_COUNT || Time.time >= _lastAttackTime + _comboWindow)
                _comboCounter = 0;

            _renderer.SetParam(_player.ComboCounterParam, _comboCounter);
            _mover.CanManualMove = false; //움직이지 못하게
            _mover.StopImmediately(true);

            float atkDirection = _renderer.FacingDirection;
            float xInput = _player.PlayerInput.InputDirection.x;

            if (Mathf.Abs(xInput) > 0)
                atkDirection = Mathf.Sign(xInput);

            Vector2 movement = _player.atkMovement[_comboCounter];
            movement.x *= atkDirection;
            _mover.AddForceToEntity(movement);
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            ++_comboCounter;
            _lastAttackTime = Time.time;
            _mover.CanManualMove = true;
            base.Exit();
        }
    }
}