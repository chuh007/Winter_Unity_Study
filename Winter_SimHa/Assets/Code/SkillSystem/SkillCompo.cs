using System;
using System.Collections.Generic;
using System.Linq;
using Code.Combats;
using Code.Core.EventSystems;
using Code.Entities;
using UnityEngine;

namespace Code.SkillSystem
{
    public class SkillCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        public Skill activeSkill; //현재 활성화되어 있는 스킬
        public ContactFilter2D whatIsEnemy;
        public Collider2D[] colliders;
        
        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }
        [field: SerializeField] public GameEventChannelSO UIChannel { get; private set; }
        [SerializeField] private int maxCheckEnemy;

        private Entity _entity;

        private Dictionary<Type, Skill> _skills;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            colliders = new Collider2D[maxCheckEnemy];
            
            _skills = new Dictionary<Type, Skill>();
            GetComponentsInChildren<Skill>().ToList().ForEach(skill => _skills.Add(skill.GetType(), skill));
            _skills.Values.ToList().ForEach(skill => skill.InitializeSkill(_entity, this));
        }
        
        public void AfterInit()
        {
            UIChannel.AddListener<SkillUpgradeClickEvent>(HandleUpgrade);
        }

        private void OnDestroy()
        {
            UIChannel.RemoveListener<SkillUpgradeClickEvent>(HandleUpgrade);   
        }

        private void HandleUpgrade(SkillUpgradeClickEvent evt)
        {
            //여기서 원래는 플레이어에 레벨업해서 스킬 포인트가 있는지를 검사하는 로직이 반드시 있어야해. 
            Type skillType = evt.targetSkill.GetType();

            if (_skills.TryGetValue(skillType, out Skill targetSkill))
            {
                if (targetSkill.CanUpgradeSkill(evt.upgradeDataSO) == false) return;  //업그레이드가 가능할 경우에만 업글
                targetSkill.UpgradeSkill(evt.upgradeDataSO); //실제 업그레이드가 이루어 질거다.
                UpdateSkillTree();
            }
        }

        private void Start()
        {
            //원래 여기 해줘야할것들이 잔뜩있는데 다 패스

            UpdateSkillTree();
        }

        private void UpdateSkillTree()
        {
             UIChannel.RaiseEvent(UIEvents.SkillTreeUpdateEvent.Initializer(_skills));
        }

        public T GetSkill<T>() where T : Skill
        {
            Type type = typeof(T);
            return _skills.GetValueOrDefault(type) as T;
        }

        public virtual int GetEnemiesInRange(Transform checkTransform, float range)
            => Physics2D.OverlapCircle(checkTransform.position, range, whatIsEnemy, colliders);
        
        public virtual int GetEnemiesInRange(Vector3 checkPosition, float range)
            => Physics2D.OverlapCircle(checkPosition, range, whatIsEnemy, colliders);

        public virtual Transform FindClosestEnemy(Vector3 checkPosition, float range)
        {
            Transform closestOne = null;
            int cnt = Physics2D.OverlapCircle(checkPosition, range, whatIsEnemy, colliders);
            
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < cnt; i++)
            {
                if (colliders[i].TryGetComponent(out Entity enemy))
                {
                    if (enemy.IsDead) continue;
                    float distanceToEnemy = Vector2.Distance(checkPosition, colliders[i].transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestOne = colliders[i].transform;
                    }
                }
            }
            return closestOne;
        }

        public void ApplyAttackFeedback(AttackDataSO attackData)
        {
            PlayerChannel.RaiseEvent( PlayerEvents.SkillFeedbackEvent.Initializer(attackData));
        }

        
    }
}