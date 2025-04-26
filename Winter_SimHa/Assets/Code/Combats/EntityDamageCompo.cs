using Code.Core.StatSystem;
using Code.Entities;
using Code.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Combats
{
    public class EntityDamageCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO criticalStat;
        [SerializeField] private StatSO criticalDamageStat;
        
        protected Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }


        public void AfterInit()
        {
            EntityStat statCompo = _entity.GetCompo<EntityStat>();
            criticalStat = statCompo.GetStat(criticalStat);
            criticalDamageStat = statCompo.GetStat(criticalDamageStat);
        }
        
        public DamageData CalculateDamage(AttackDataSO attackData, StatSO majorStat, float multiplier = 1)
        {
            DamageData damageData = new DamageData();
            damageData.damage = majorStat.Value * attackData.damageMultiplier * multiplier + attackData.damageIncrease;
            if (Random.value < criticalStat.Value)
            {
                damageData.isCritical = true;
                damageData.damage *= criticalDamageStat.Value;
            }
            else
            {
                damageData.isCritical = false;
            }
            return damageData;
        }
    }
}