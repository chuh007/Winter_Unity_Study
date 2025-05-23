﻿using System;
using Code.Animators;
using Code.Combats;
using Code.Entities;
using Code.Players;
using DG.Tweening;
using UnityEngine;

namespace Code.SkillSystem.Crystals
{
    public abstract class Crystal : MonoBehaviour
    {
        [SerializeField] protected AnimParamSO explosionParam;
        [SerializeField] protected DamageCaster damageCaster;
        [SerializeField] protected AttackDataSO attackData;

        protected bool _canExplode;
        protected Animator _animator;
        protected CrystalSkill _skill;
        protected CrystalController _crystalController;
        protected Entity _owner;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected void ExplosionAnimationEnd()
        {
            transform.DOKill(); //현재 transform에 걸린 DOTween을 모두 제거
            Destroy(gameObject);
        }

        public virtual void Explosion()
        {
            _canExplode = false;
            _animator.SetTrigger(explosionParam.hashValue);

            DamageData damage = _skill.CalculateDamage(attackData, _skill.damageMultiplier, _skill.DamageStat);
            
            if (damageCaster.CastDamage(damage, attackData.knockBackForce, attackData.isPowerAttack))
            {
                //스킬에 공격 피드백들을 실행한다.
                _skill.skillCompo.ApplyAttackFeedback(attackData);
            }
        }

        public virtual void SetUp(CrystalSkill skill, CrystalController controller, Entity owner)
        {
            _skill = skill;
            _owner = owner;
            damageCaster.InitCaster(owner);
            _canExplode = true;
            _crystalController = controller;
        }
        
        public abstract void TriggerCrystal();
    }
}