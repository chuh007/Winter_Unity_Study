using System;
using System.Collections.Generic;
using Code.Animators;
using Code.Combats;
using Code.Core.StatSystem;
using Code.Entities;
using NUnit.Framework;
using UnityEngine;

namespace Code.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO attackSpeedStat;
        [SerializeField] private AnimParamSO atkSpeedParam;
        [SerializeField] private DamageCaster damageCaster;

        [SerializeField] private List<AttackDataSO> attackDataList;

        [Header("Counter attack setting")]
        public float counterAttackDuration;
        public AnimParamSO successCounterParam;
        public LayerMask whatIsCounterable;

        private Player _player;
        private EntityStat _statCompo;
        private EntityRenderer _renderer;
        private EntityMover _mover;
        private EntityAnimationTrigger _triggerCompo;

        private bool _canJumpAttack;

        private Dictionary<string, AttackDataSO> _attackDataDictionary;
        private AttackDataSO _currentAttackData;

        #region Init section

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _statCompo = entity.GetCompo<EntityStat>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _mover = entity.GetCompo<EntityMover>();
            _triggerCompo = entity.GetCompo<EntityAnimationTrigger>();
            damageCaster.InitCaster(entity);

            _attackDataDictionary = new Dictionary<string, AttackDataSO>();
            attackDataList.ForEach(attackData => _attackDataDictionary.Add(attackData.attackName, attackData));
        }

        public void AfterInit()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange += HandleAttackSpeedChange;
            _renderer.SetParam(atkSpeedParam, _statCompo.GetStat(attackSpeedStat).Value);

            _triggerCompo.OnAttackTrigger += HandleAtackTrigger;
        }

        

        private void OnDestroy()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange -= HandleAttackSpeedChange;
            _triggerCompo.OnAttackTrigger -= HandleAtackTrigger;
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

        public AttackDataSO GetAttackData(string attackName)
        {
             AttackDataSO data = _attackDataDictionary.GetValueOrDefault(attackName);
            Debug.Assert(data != null,$"request attack data is not exist : {attackName}");
            return data;
        }

        public void SetAttackData(AttackDataSO attackData)
        {
            _currentAttackData = attackData;
        }

        private void FixedUpdate()
        {
            if (_canJumpAttack == false && _mover.IsGroundDetected())
                _canJumpAttack = true;
        }

        private void HandleAtackTrigger()
        {
            float damage = 5f;
            Vector2 knockBackForce = _currentAttackData.knockBackForce;
            bool success = damageCaster.CastDamage(damage, knockBackForce, _currentAttackData.isPowerAttack);

            if (success)
            {
                Debug.Log($"<color=red>Damaged! - {damage}</color>");
            }
        }

        public ICounterable GetCounterableTargetInRadius()
        {
            Vector3 center = damageCaster.transform.position;
            Collider2D collider = Physics2D.OverlapCircle(center, damageCaster.damageRadius, whatIsCounterable);
            if (collider != null)
                return collider.GetComponent<ICounterable>();

            return default;
        }
    }
}