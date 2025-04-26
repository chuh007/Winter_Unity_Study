using Code.Core.StatSystem;
using Code.Entities;
using Code.Players;
using UnityEngine;

namespace Code.Combats
{
    public class EntityDamageCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO criticalStat;
        [SerializeField] private StatSO criticalDamageStat;
        
        private Entity _entity;
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
        
        public DamageData CalculateDamage(AttackDataSO attackData, StatSO majorStat, float multiplier = 1f)
        {
            DamageData damageData = new DamageData();
            damageData.damage = majorStat.Value * attackData.damageMultiplier * multiplier + attackData.damageIncrease;
            //0 ~ 1 을 뽑는 녀석
            if (Random.value < criticalStat.Value)  //크리티컬 발생
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