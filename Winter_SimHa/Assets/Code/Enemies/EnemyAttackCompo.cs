using Code.Combats;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent
    {
        [Header("Atk settings")] 
        public float attackDistance;
        public float detectDistance;
        
        [SerializeField] private float attackCooldown, cooldownRandomness;
        
        [Header("Reference")]
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private string attackRangeName, detectRangeName, attackCooldownName;

        private BTEnemy _enemy;
        private BlackboardVariable<float> _attackCooldownVariable;
        
        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;
            Debug.Assert(_enemy != null, $"Not corrected entity - enemy attack component [{entity.gameObject.name}]");
            _enemy.GetBlackboardVariable<float>(attackRangeName).Value = attackDistance;
            _enemy.GetBlackboardVariable<float>(detectRangeName).Value = detectDistance;
            _attackCooldownVariable = _enemy.GetBlackboardVariable<float>(attackCooldownName);
        }
        
        
    }
}