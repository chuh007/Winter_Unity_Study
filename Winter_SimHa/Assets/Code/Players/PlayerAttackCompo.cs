using System;
using System.Collections.Generic;
using Code.Animators;
using Code.Combats;
using Code.Core.EventSystems;
using Code.Core.StatSystem;
using Code.Entities;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Players
{
    public struct DamageData
    {
        public float damage;
        public bool isCritical;
    }
    
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO attackSpeedStat;
        [SerializeField] private StatSO damageStat;
       
        [SerializeField] private AnimParamSO atkSpeedParam;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private List<AttackDataSO> attackDataList;

        [Header("Counter attack settings")] 
        public float counterAttackDuration;
        public AnimParamSO successCounterParam;
        public LayerMask whatIsCounterable;
        
        private Player _player;
        private EntityStat _statCompo;
        private EntityRenderer _renderer;
        private EntityMover _mover;
        private EntityAnimationTrigger _triggerCompo;
        private EntityDamageCompo _damageCompo;
        
        private bool _canJumpAttack;

        private Dictionary<string, AttackDataSO> _attackDataDictionary;
        private AttackDataSO _currentAttackData;


        #region Init section

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _statCompo = entity.GetCompo<EntityStat>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _mover = entity.GetCompo<EntityMover>();
            _triggerCompo = entity.GetCompo<EntityAnimationTrigger>();
            _damageCompo = entity.GetCompo<EntityDamageCompo>();
            damageCaster.InitCaster(entity);

            //리스트를 딕셔너리로 변경한다.
            _attackDataDictionary = new Dictionary<string, AttackDataSO>();
            attackDataList.ForEach(attackData => _attackDataDictionary.Add(attackData.attackName, attackData));
        }

        public void AfterInit()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange += HandleAttackSpeedChange;
            damageStat = _statCompo.GetStat(damageStat);
            
            
            _renderer.SetParam(atkSpeedParam, _statCompo.GetStat(attackSpeedStat).Value);

            _triggerCompo.OnAttackTrigger += HandleAttackTrigger;
            _player.PlayerChannel.AddListener<SkillFeedbackEvent>(HandleSkillFeedback);
        }

        private void OnDestroy()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange -= HandleAttackSpeedChange;
            _triggerCompo.OnAttackTrigger -= HandleAttackTrigger;
            _player.PlayerChannel.AddListener<SkillFeedbackEvent>(HandleSkillFeedback);
        }


        #endregion

        private void HandleAttackSpeedChange(StatSO stat, float current, float previous)
        {
            _renderer.SetParam(atkSpeedParam, current);
        }

        public bool CanJumpAttack()
        {
            bool returnValue = _canJumpAttack;
            if (_canJumpAttack)
                _canJumpAttack = false;
            return returnValue;
        }
        
        private void FixedUpdate()
        {
            if (_canJumpAttack == false && _mover.IsGroundDetected())
                _canJumpAttack = true;
        }

        public AttackDataSO GetAttackData(string attackName)
        {
            AttackDataSO data = _attackDataDictionary.GetValueOrDefault(attackName);
            Debug.Assert(data != null, $"request attack data is not exist : {attackName}");
            return data;
        }
        
        public void SetAttackData(AttackDataSO attackData)
        {
            _currentAttackData = attackData;
        }
        
        private void HandleAttackTrigger()
        {
            DamageData damageData = CalculateDamage(_currentAttackData); // TODO 개선 
            Vector2 knockBackForce = _currentAttackData.knockBackForce;
            bool success = damageCaster.CastDamage(damageData, knockBackForce, _currentAttackData.isPowerAttack);

            if (success)
            {
                string color = damageData.isCritical ? "red" : "white";
                Debug.Log($"<color={color}>Damaged! - {damageData.damage}</color>");
                _player.PlayerChannel.RaiseEvent(PlayerEvents.PlayerAttackSuccess);
                GenerateAttackFeedback(_currentAttackData);
            }
        }

        private void GenerateAttackFeedback(AttackDataSO attackData)
        {
            impulseSource.ImpulseDefinition.ImpulseDuration = attackData.cameraShakeDuration;
            impulseSource.GenerateImpulse(attackData.cameraShakePower);
        }
        
        private void HandleSkillFeedback(SkillFeedbackEvent evt)
        {
            GenerateAttackFeedback(evt.attackData);
        }

        public ICounterable GetCounterableTargetInRadius()
        {
            Vector3 center = damageCaster.transform.position;
            Collider2D collider = damageCaster.GetCounterableTarget(center, whatIsCounterable);
            //Collider2D collider = Physics2D.OverlapCircle(center, damageCaster.GetSize(), whatIsCounterable);
            if(collider != null)
                return collider.GetComponent<ICounterable>();
            
            return default;
        }

        public DamageData CalculateDamage(AttackDataSO attackData, float multiplier = 1, StatSO majorStat = null)
        {
            if (majorStat == null)
                majorStat = damageStat;
            return _damageCompo.CalculateDamage(attackData, majorStat, multiplier);
        }

        
    }
}