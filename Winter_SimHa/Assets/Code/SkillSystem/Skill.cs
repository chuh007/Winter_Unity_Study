using System.Linq;
using System.Collections.Generic;
using Code.Combats;
using Code.Core.StatSystem;
using Code.Entities;
using Code.Players;
using UnityEngine;

namespace Code.SkillSystem
{
    public delegate void CooldownInfo(float current, float totalTime);
    
    public abstract class Skill : MonoBehaviour
    {
        [field: SerializeField] public SkillDataSO SkillData { get; private set; }
        public bool skillEnabled = false;
        
        [SerializeField] protected float cooldown;
        protected float _cooldownTimer;
        protected Entity _entity;
        protected PlayerAttackCompo _attackCompo;
        [HideInInspector] public SkillCompo skillCompo;
        

        public bool IsCooldown => _cooldownTimer > 0f;
        public event CooldownInfo OnCooldown;

        #region Upgrade Skill

        public List<SkillPerkUpgradeSO> upgradedList = new List<SkillPerkUpgradeSO>(); //업글이 된 애들은 여기에 Add된다.

        public int GetUpgradeCount(SkillPerkUpgradeSO upgradeData)
            => upgradedList.Count(upgrade => upgrade == upgradeData);

        public void UpgradeSkill(SkillPerkUpgradeSO upgradeData)
        {
            upgradedList.Add(upgradeData);
            upgradeData.UpgradeSkill(this); //이 스킬을 업그레이드
        }

        public void RollbackUpgrade(SkillPerkUpgradeSO upgradeData)
        {
            upgradedList.Remove(upgradeData);
            upgradeData.RollbackUpgrade(this);
        }

        //이 업그레이드가 수행가능하냐?
        public bool CanUpgradeSkill(SkillPerkUpgradeSO upgradeData)
        {
            foreach (var data in upgradeData.needUpgradeList)
            {
                if (upgradedList.Contains(data) == false) return false;
            }

            foreach (var data in upgradeData.dontNeedUpgradeList)
            {
                if (upgradedList.Contains(data)) return false;
            }

            int currentUpgradedCnt = GetUpgradeCount(upgradeData);
            if (currentUpgradedCnt >= upgradeData.maxUpgradeCount)
                return false;

            return true;
        }
        
        #endregion

        public virtual void InitializeSkill(Entity entity, SkillCompo skillCompo)
        {
            _entity = entity;
            this.skillCompo = skillCompo;
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }

        public DamageData CalculateDamage(AttackDataSO attackData, float skillMultiplier, StatSO majorStat)
        {
            return _attackCompo.CalculateDamage(attackData, skillMultiplier, majorStat);
        }

        protected virtual void Update()
        {
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;

                if (_cooldownTimer <= 0)
                    _cooldownTimer = 0;
                
                OnCooldown?.Invoke(_cooldownTimer, cooldown);
            }
        }

        public virtual bool AttemptUseSkill()
        {
            if (_cooldownTimer <= 0 && skillEnabled)
            {
                _cooldownTimer = cooldown;
                UseSkill();
                return true;
            }
            Debug.Log("Skill cooldown or locked");
            return false;
        }

        public virtual void UseSkill()
        {
            //여기서 나중에 스킬을 썼음을 알려주는 피드백이 필요하다.
        }

        public virtual void UseSkillWithoutCooltimeAndEffect()
        {
            //자동발동 스킬들이 이용하기 위해 만든 함수.
        }
    }
}