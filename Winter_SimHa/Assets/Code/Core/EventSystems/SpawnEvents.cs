using UnityEngine;


namespace Code.Core.EventSystems
{
    public static class SpawnEvents
    {
        public static SpawnAnimationEffect SpawnAnimationEffect = new SpawnAnimationEffect();
    }

    public class SpawnAnimationEffect : GameEvent
    {
        public PoolTypeSO poolType;
        public Vector3 position;
        public Quaternion roatation;
        public Vector3 scale;
        public Color effectColor;

        public SpawnAnimationEffect Initializer(PoolTypeSO poolTyep, Vector3 position, Quaternion rotation,
            Vector3 scale, Color color)
        {
            this.poolType = poolTyep;
            this.position = position;
            this.roatation = rotation;
            this.scale = scale;
            this.effectColor = color;
            return this;
        }
    }
}