using System;
using System.Collections.Generic;
using Code.SkillSystem;
using Code.UI.MainMenu;

namespace Code.Core.EventSystems
{
    public static class UIEvents
    {
        public static readonly FadeEvent FadeEvent = new FadeEvent();
        public static readonly FadeCompleteEvent FadeCompleteEvent = new FadeCompleteEvent();
        public static readonly OpenMenuEvent OpenMenuEvent = new OpenMenuEvent();
        public static readonly ChangeMenuEvent ChangeMenuEvent = new ChangeMenuEvent();
        public static readonly SkillTreeUpdateEvent SkillTreeUpdateEvent = new SkillTreeUpdateEvent();
        public static readonly SkillUpgradeClickEvent SkillUpgradeClickEvent = new SkillUpgradeClickEvent();
    }

    public class SkillUpgradeClickEvent : GameEvent
    {
        public Skill targetSkill;
        public SkillPerkUpgradeSO upgradeDataSO;

        public SkillUpgradeClickEvent Initializer(Skill skill, SkillPerkUpgradeSO upgradeDataSO)
        {
            this.targetSkill = skill;
            this.upgradeDataSO = upgradeDataSO;
            return this;
        }
    }

    public class FadeEvent : GameEvent
    {
        public bool isFadeIn;
        public float fadeTime;
        public bool isSaveOrLoad; //저장이나 로딩을 하는거냐?
    }

    public class FadeCompleteEvent : GameEvent { }

    public class OpenMenuEvent : GameEvent
    {
        public MenuUITypeSO UIType;
    }

    public class ChangeMenuEvent : GameEvent
    {
        public MenuUITypeSO UIType;
    }

    public class SkillTreeUpdateEvent : GameEvent
    {
        public Dictionary<Type, Skill> skills;

        public SkillTreeUpdateEvent Initializer(Dictionary<Type, Skill> skills)
        {
            this.skills = skills;
            return this;
        }
    }
}