using System.Collections.Generic;
using UnityEngine;

namespace Code.SkillSystem
{
    public enum SkillType
    {
        Default = 0, Passive = 1, Select = 2 
    }
    [CreateAssetMenu(fileName = "SkillData", menuName = "SO/Skill/SkillData", order = 0)]
    public class SkillDataSO : ScriptableObject
    {
        public string skillName;
        public SkillType skillType = SkillType.Default;
        public Sprite skillIcon;

        public List<SkillPerkUpgradeSO> skillUpgradeList;
    }
}