using Code.Entities;
using UnityEngine;

namespace Code.Combats
{
    public interface ICounterable
    {
        public bool CanCounter {  get; }

        public Transform TargetTrm { get; }

        public void ApplyCounter(float damage, Vector2 direction, Vector2 knockBackForce,
                                    bool isPowerAttack, Entity dealer);
    }
}

