using Code.Entities;
using Code.Players;
using UnityEngine;

namespace Code.Combats
{
    public interface ICounterable
    {
        public bool CanCounter { get;}
        
        public Transform TargetTrm { get; }
        
        //데미지라는 float를 DamageData라는 구조체로 넘길꺼야.
        public void ApplyCounter(DamageData damageData, Vector2 direction, Vector2 knockBackForce, 
                                    bool isPowerAttack, Entity dealer);
    }
}