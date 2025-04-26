using System;
using Code.Core.EventSystems;
using Code.SkillSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class SkillUpgradeBtnUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private Image upgrandeIcon;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Color lockColor;
        
        private Skill _targetSkill;
        public SkillPerkUpgradeSO upgradeData;

        private void OnValidate()
        {
            if(upgradeData == null) return;
            
            gameObject.name = $"SkillUpgradeBtn_{upgradeData.upgradeName}";
            if(upgrandeIcon != null)
                upgrandeIcon.sprite = upgradeData.upgradeIcon;
        }

        private void Awake()
        {
            upgrandeIcon.color = lockColor;
            upgradeButton.onClick.AddListener(HandleUpgradeClick);
        }

        private void HandleUpgradeClick()
        {
            if(upgradeData == null) return;
            var evt = UIEvents.SkillUpgradeClickEvent.Initializer(_targetSkill, upgradeData);
            uiChannel.RaiseEvent(evt);
        }

        public void SetTargetSkill(Skill skill) => _targetSkill = skill;

        public void UnlockIcon(bool isUnlocked)
        {
            upgrandeIcon.color = isUnlocked ? Color.white : lockColor;
        }

        public void UpdateUpgradeText(int count)
        {
            if(upgradeData.maxUpgradeCount > 1)
                upgradeText.text = $"{count}/{upgradeData.maxUpgradeCount}";
            else
                upgradeText.text = string.Empty;
        }
    }
}