using System;
using System.Collections.Generic;
using Code.Core.StatSystem;
using Code.Entities;
using UnityEngine;

namespace Code.Items
{
    public enum EquipType
    {
        Weapon = 1, Armor = 2, Amulet = 3, Ring = 4
    }

    [Serializable]
    public struct AddingStat  //이 아이템을 착용했을 때 늘어나는 능력치
    {
        public StatSO targetStat;
        public float modifyValue; 
    }
    
    [CreateAssetMenu(fileName = "EquipItemData", menuName = "SO/Items/EquipItem", order = 0)]
    public class EquipItemDataSO : ItemDataSO
    {
        public EquipType equipType;
        public List<AddingStat> addingStats;
        [TextArea] public string itemEffectDescription;
        
        private int _descriptionLength;

        public void AddModifier(EntityStat statCompo)
        {
            foreach (AddingStat stat in addingStats)
            {
                StatSO targetStat = statCompo.GetStat(stat.targetStat); //목표 스텟을 가져온다.
                if(targetStat != null)
                    targetStat.AddModifier(this, stat.modifyValue);
            }
        }

        public void RemoveModifier(EntityStat statCompo)
        {
            foreach (AddingStat stat in addingStats)
            {
                StatSO targetStat = statCompo.GetStat(stat.targetStat); //목표 스텟을 가져온다.
                if(targetStat != null)
                    targetStat.RemoveModifier(this);
            }
        }

        public override string GetDescription()
        {
            _stringBuilder.Clear();
            _descriptionLength = 0;
            foreach (var stat in addingStats)
            {
                AddItemDescription(stat.targetStat.statName, stat.modifyValue);
            }

            if (_descriptionLength < 5)
            {
                for (int i = _descriptionLength; i < 5; i++)
                {
                    _stringBuilder.AppendLine();
                    _stringBuilder.Append(""); //5줄 채워주기
                }
            }

            if (!string.IsNullOrEmpty(itemEffectDescription))
            {
                _stringBuilder.AppendLine();
                _stringBuilder.Append(itemEffectDescription);
            }
            return _stringBuilder.ToString();
        }

        private void AddItemDescription(string targetStatStatName, float statModifyValue)
        {
            //다국어 지원 서비스를 만든다면 여기에 추가적으로 구현이 필요하다.
            if (Mathf.Approximately(statModifyValue, 0) == false)
            {
                if(_stringBuilder.Length > 0)
                    _stringBuilder.AppendLine();
                
                ++ _descriptionLength;
                _stringBuilder.Append($"{targetStatStatName} : {statModifyValue}");
            }
        }
    }
}