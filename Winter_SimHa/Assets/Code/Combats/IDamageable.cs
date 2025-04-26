using Code.Entities;
using Code.Players;
using UnityEngine;

namespace Code.Combats
{
    public interface IDamageable
    {
        public void ApplyDamage(DamageData damageData, Vector2 direction, Vector2 knockBackPower, bool isPowerAttack, Entity dealer);
    }
}