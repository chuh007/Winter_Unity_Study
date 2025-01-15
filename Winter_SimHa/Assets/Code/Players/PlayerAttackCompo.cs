using System;
using Code.Animators;
using Code.Core.StatSystem;
using Code.Entities;
using UnityEngine;

namespace Code.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO attackSpeedStat;
        [SerializeField] private AnimParamSO atkSpeedParam;

        private Player _player;
        private EntityStat _statCompo;
        private EntityRenderer _renderer;
        private EntityMover _mover;

        private bool _canJumpAttack;

        #region Init section

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _statCompo = entity.GetCompo<EntityStat>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _mover = entity.GetCompo<EntityMover>();
        }

        public void AfterInit()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange += HandleAttackSpeedChange;
            _renderer.SetParam(atkSpeedParam, _statCompo.GetStat(attackSpeedStat).Value);
        }

        private void OnDestroy()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange -= HandleAttackSpeedChange;
        }
        
        #endregion

        private void HandleAttackSpeedChange(StatSO stat, float current, float previous)
        {
            _renderer.SetParam(atkSpeedParam, current);
        }

        public bool CanJumpAttack()
        {
            bool returnValue = _canJumpAttack;
            if (_canJumpAttack)
                _canJumpAttack = false;
            return returnValue;
        }

        private void FixedUpdate()
        {
            if (_canJumpAttack == false && _mover.IsGroundDetected())
                _canJumpAttack = true;
        }
    }
}