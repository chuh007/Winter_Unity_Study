using System;
using Code.Entities;
using UnityEngine;

namespace Code.Combats
{
    public class DamageCaster : MonoBehaviour //나중에 추상형 클래스로 변경되게 된다.
    {
        public float damageRadius;

        [SerializeField] private int maxHitCount = 1; //최대 피격 가능 객체 수
        [SerializeField] private ContactFilter2D contactFilter;
        private Collider2D[] hitResults;

        private Entity _owner;

        public void InitCaster(Entity owner)
        {
            hitResults = new Collider2D[maxHitCount];
            _owner = owner;
        }

        public bool CastDamage(float damage, Vector2 knockBack, bool isPowerAttack)
        {
            int cnt = Physics2D.OverlapCircle(transform.position, damageRadius, contactFilter, hitResults);

            for (int i = 0; i < cnt; i++)
            {
                //피격 방향 구하기 ^_^
                Vector2 direction = (hitResults[i].transform.position - _owner.transform.position).normalized;
                //실제 데미지와넉백은 여기서 차후에 구현합니다.
                knockBack.x *= Mathf.Sign(direction.x);

                if (hitResults[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(damage, direction, knockBack, isPowerAttack, _owner);
                }
                
            }

            return cnt > 0;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.7f, 0.7f, 0, 1f);
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
#endif
    }
}