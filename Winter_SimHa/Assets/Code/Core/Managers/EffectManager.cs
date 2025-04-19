using System;
using Code.Core.EventSystems;
using UnityEngine;

namespace Code.Core.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO spawnChannel;
        [SerializeField] private PoolManagerSO poolManager;

        private void Awake()
        {
            spawnChannel.AddListener<SpawnAnimationEffect>(HandleSpawnAnimationEffect);
        }

        private void OnDestroy()
        {
            spawnChannel.RemoveListener<SpawnAnimationEffect>(HandleSpawnAnimationEffect);             
        }

        private void HandleSpawnAnimationEffect(SpawnAnimationEffect evt)
        {
            AnimatorEffect effect = poolManager.Pop(evt.poolType) as AnimatorEffect;
            effect.PlayAnimation(evt.position, evt.rotation, evt.scale);
            effect.SetEffectColor(evt.effectColor);
        }
    }
}