using Code.Core.StatSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Entities
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private StatSO[] _stats;

        public Entity Owner { get; private set; }

        public void Initialize(Entity entity)
        {
            Owner = entity;
            _stats = statOverrides.Select(stat => stat.CreatStat()).ToArray();
        }

        public StatSO GetStat(StatSO targetStat)
        {
            Debug.Assert(targetStat != null,"Stat::GetStat : target stat is null");
            return _stats.FirstOrDefault(stat => stat.statName == targetStat.statName);
        }

        public bool TryGetStat(StatSO targetStat, out StatSO outStat)
        {
            Debug.Assert(targetStat != null, "Stats::GetStat : target stat is null");

            outStat = _stats.FirstOrDefault(stat => stat.statName == targetStat.statName);
            return outStat;
        }

        public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat) => GetStat(stat).BaseValue;
        public void IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;
        public void AddModifier(StatSO stat, object key, float value) => GetStat(stat).AddModifier(key, value);
        public void RemoveModifier(StatSO stat, object key) => GetStat(stat).RemoveModifier(key);

        public void CleanAllModifier()
        {
            foreach(StatSO stat in _stats)
            {
                stat.ClearAllModifier();
            }
        }
    }
}

