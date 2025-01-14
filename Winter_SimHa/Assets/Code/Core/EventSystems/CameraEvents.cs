using UnityEngine;

namespace Code.Core.EventSystems
{
    public class CameraEvents
    {
        public static PanEvent panEvent = new PanEvent();
    }

    public class PanEvent : GameEvent
    {
        public float distance;
        public float panTime;
        public Vector2 direction;
        public bool isRewindToStart;
    }
}

