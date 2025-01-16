using Code.Enemies;
using Code.Entities;
using UnityEngine;

namespace Code.Combats
{
    public class DamageCaster : MonoBehaviour
    {
        public float damageRadius;

        [SerializeField] private int maxHitCount = 1;
        [SerializeField] private ContactFilter2D contactFilter;
        private Collider2D[] hitResults;

        private Entity _onwer;

        public void InitCaster(Entity owner)
        {
            hitResults = new Collider2D[maxHitCount];
            _onwer = owner;
        }

        public bool CastDamage(float damage, Vector2 knockback, bool isPowerAttack)
        {
            int cnt = Physics2D.OverlapCircle(transform.position, damageRadius, contactFilter, hitResults);

            for(int i = 0; i < cnt; i++)
            {
                Vector2 direction = (hitResults[i].transform.position - _onwer.transform.position).normalized;
                knockback.x = Mathf.Sign(direction.x);

                if (hitResults[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(damage, direction, knockback,isPowerAttack, _onwer);
                }

            }

            return cnt > 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
#endif
    }
}

