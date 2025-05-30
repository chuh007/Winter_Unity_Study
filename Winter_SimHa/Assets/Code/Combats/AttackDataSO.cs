﻿using System;
using UnityEngine;

namespace Code.Combats
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public string attackName;
        public Vector2 movement;
        public Vector2 knockBackForce;
        public float damageMultiplier = 1f;
        public float damageIncrease = 0;
        public bool isPowerAttack;

        public float cameraShakePower;
        public float cameraShakeDuration;

        private void OnEnable()
        {
            attackName = this.name;
        }
    }
}