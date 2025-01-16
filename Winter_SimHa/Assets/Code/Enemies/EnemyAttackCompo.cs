using Code.Combats;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent
    {
        [Header("Atk setting")]
        public float attackDistance;
        public float detectDistance;

        [SerializeField] private float attackCooldown, cooldownRandomness;

        [Header("Reference")]
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private string attackRangeName, detectRangeName, attackCooldownName;

        private BTEnemy _enemy;
        private BlackboardVariable<float> _atttackCooldownVariable;

        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;
            Debug.Assert(entity != null,$"Not corrected entity - enemy attack component [{entity.gameObject.name}]");
            _enemy.GetBlackboardVariable<float>(attackRangeName).Value = attackDistance;
            _enemy.GetBlackboardVariable<float>(detectRangeName).Value = detectDistance;
            _atttackCooldownVariable = _enemy.GetBlackboardVariable<float>(attackCooldownName);
        }
    }
}

