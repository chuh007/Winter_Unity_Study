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
        [SerializeField] private Image upgradeIcon;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Color lockColor; //스킬업글이 잠겨있을 때 나올 색상

        private Skill _targetSkill;
        public SkillPerkUpgradeSO upgradeData;

        private void OnValidate()
        {
            if (upgradeData == null) return;

            gameObject.name = $"SkillUpgradeBtn_{upgradeData.upgradeName}";
            if (upgradeIcon != null)
                upgradeIcon.sprite = upgradeData.upgradeIcon;
        }

        private void Awake()
        {
            upgradeIcon.color = lockColor; //처음 시작하면 잠긴 컬러로 셋
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
            upgradeIcon.color = isUnlocked ? Color.white : lockColor;
        }

        public void UpdateUpgradeText(int count)
        {
            if (upgradeData.maxUpgradeCount > 1)
                upgradeText.text = $"{count}/{upgradeData.maxUpgradeCount}";
            else
                upgradeText.text = string.Empty;
        }
        
    }
}