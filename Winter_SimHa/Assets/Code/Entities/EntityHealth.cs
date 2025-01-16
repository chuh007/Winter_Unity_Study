using Code.Core.StatSystem;
using Code.Enemies;
using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO hpStat;

        public float maxHealth;
        private float _currentHealth;

        public event Action<Vector2> OnKnockback;

        private Entity _entity;
        private EntityStat _statCompo;
        private EntityFeedbackData _feedbackData;

        #region Init

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = _entity.GetCompo<EntityStat>();
            _feedbackData = _entity.GetCompo<EntityFeedbackData>();
        }

        public void AfterInit()
        {
            _statCompo.GetStat(hpStat).OnValueChange += HandleHpChange;
            _currentHealth = maxHealth = _statCompo.GetStat(hpStat).Value;
            _entity.OnDamage += ApplyDamage;
        }
        private void OnDestroy()
        {
            _statCompo.GetStat(hpStat).OnValueChange -= HandleHpChange;
            _entity.OnDamage -= ApplyDamage;
        }

        #endregion

        private void HandleHpChange(StatSO stat, float current, float previous)
        {
            maxHealth = current;
            _currentHealth = Mathf.Clamp(_currentHealth + current - previous, 1f, maxHealth);
            // 체력 변경으로 사망하지는 않게
        }

        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockbackPower, bool isPowerAttack, Entity dealer)
        {
            if (_entity.IsDead) return;

            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
            _feedbackData.LastAttackDirection = direction.normalized;
            _feedbackData.IsLastHitPowerAttack = isPowerAttack;
            _feedbackData.LastEntityWhoHit = dealer;

            AfterHitFeedbacks(knockbackPower);
        }

        private void AfterHitFeedbacks(Vector2 knockbackPower)
        {
            _entity.OnHit?.Invoke();
            OnKnockback?.Invoke(knockbackPower);

            if(_currentHealth <= 0)
            {
                _entity.OnDead?.Invoke();
            }
        }
    }
}

