using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityAnimationTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEnd;
        public event Action OnAttackTrigger;
        public event Action<bool> OnCounterStatuschange;

        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd() => OnAnimationEnd?.Invoke();
        private void AttackTrigger() => OnAttackTrigger?.Invoke();
        private void OpenCounterWindow() => OnCounterStatuschange?.Invoke(true);
        private void CloseCounterWindow() => OnCounterStatuschange?.Invoke(false);
    }
}