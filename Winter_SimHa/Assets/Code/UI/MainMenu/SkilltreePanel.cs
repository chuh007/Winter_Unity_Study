using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Code.Core.EventSystems;
using Code.SkillSystem;
using UnityEngine;

namespace Code.UI.MainMenu
{
    public class SkillTreePanel : MenuPanel
    {
        [field: SerializeField] public GameEventChannelSO UIChannel { get; private set; }
        [SerializeField] private Transform skillTreeParentTrm;

        private Dictionary<SkillPerkUpgradeSO, SkillUpgradeBtnUI> _treeBtns;

        private void Awake()
        {
            UIChannel.AddListener<SkillTreeUpdateEvent>(HandleSkillTreeUpdate);
            _treeBtns = new Dictionary<SkillPerkUpgradeSO, SkillUpgradeBtnUI>();
            skillTreeParentTrm.GetComponentsInChildren<SkillUpgradeBtnUI>().ToList()
                .ForEach(btnUI => _treeBtns.Add(btnUI.upgradeData, btnUI));
        }

        private void OnDestroy()
        {
            UIChannel.RemoveListener<SkillTreeUpdateEvent>(HandleSkillTreeUpdate);
        }

        private void HandleSkillTreeUpdate(SkillTreeUpdateEvent evt)
        {
            foreach (var skill in evt.skills.Values)  //넘겨진 스킬 딕셔너리의 값들을 순회하면서
            {
                if(skill.SkillData == null) continue;
                foreach (var upgradeData in skill.SkillData.skillUpgradeList)
                {
                    if (_treeBtns.TryGetValue(upgradeData, out var btnUI))
                    {
                        int cnt = skill.GetUpgradeCount(upgradeData);
                        
                        btnUI.UnlockIcon(cnt > 0);
                        btnUI.UpdateUpgradeText(cnt);
                        btnUI.SetTargetSkill(skill);
                    }
                }
            }
        }
    }
}