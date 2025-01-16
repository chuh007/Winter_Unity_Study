using Code.Entities;
using UnityEngine;

namespace Code.Enemies
{
    public interface IDamageable
    {
        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockbackPower,bool isPowerAttack, Entity dealer);
    }
}

