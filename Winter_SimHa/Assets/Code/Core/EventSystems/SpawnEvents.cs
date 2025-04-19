using UnityEngine;

namespace Code.Core.EventSystems
{
    public static class SpawnEvents
    {
        public static readonly SpawnAnimationEffect SpawnAnimationEffect = new SpawnAnimationEffect();
    }

    public class SpawnAnimationEffect : GameEvent
    {
        public PoolTypeSO poolType;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public Color effectColor;

        public SpawnAnimationEffect Initializer(PoolTypeSO poolType, Vector3 position, Quaternion rotation,
            Vector3 scale, Color effectColor)
        {
            this.poolType = poolType;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.effectColor = effectColor;
            return this;
        }
    }
}