using Code.Core.EventSystems;
using Code.Entities;
using UnityEngine;

namespace Code.Feedbacks
{
    public class HitImpactFeedback : Feedback
    {
        [SerializeField] private PoolTypeSO impactPoolType, slashPoolType;
        [SerializeField] private EntityFeedbackData feedbackData;
        [SerializeField, ColorUsage(true, true)] private Color impactColor;
        [SerializeField] private Vector3 effectScale = new Vector3(2f, 2f, 2f);
        [SerializeField] private GameEventChannelSO spawnChannel;
        public override void CreateFeedback()
        {
            var evt = SpawnEvents.SpawnAnimationEffect;
            Vector2 direction = feedbackData.LastAttackDirection;
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion towardRotation = Quaternion.Euler(0, 0, zRotation);
            evt.Initializer(impactPoolType, transform.position + (Vector3)(Random.insideUnitCircle * 0.5f), towardRotation, effectScale, impactColor);
            
            spawnChannel.RaiseEvent(evt);

            if (feedbackData.IsLastHitPowerAttack) //마지막 공격을 파워어택으로 맞았다면 슬래시 이펙트도 나오게
            {
                evt.poolType = slashPoolType;
                spawnChannel.RaiseEvent(evt);
            }
        }
    }
}