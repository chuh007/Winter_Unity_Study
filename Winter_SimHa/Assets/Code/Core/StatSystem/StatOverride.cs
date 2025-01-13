using System;
using UnityEngine;

namespace Code.Core.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [SerializeField] private StatSO stat;
        [SerializeField] private bool isUseOverride;
        [SerializeField] private float overrideValue;

        public StatOverride(StatSO stat) => this.stat = stat;

        public StatSO CreatStat()
        {
            StatSO newStat = stat.Clone() as StatSO;
            Debug.Assert(newStat != null, $"{stat.statName} clone faild");

            if(isUseOverride)
                newStat.BaseValue = overrideValue;

            return newStat;
        }
    }
}

