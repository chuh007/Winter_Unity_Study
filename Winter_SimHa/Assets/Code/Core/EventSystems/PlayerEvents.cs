using UnityEngine;

namespace Code.Core.EventSystems
{
    public static class PlayerEvents
    {
        public static AddEXPEvent AddExpEvent = new AddEXPEvent();
        public static PlayerAttackSuccess PlayerAttackSuccess = new PlayerAttackSuccess();
        public static DashStartEvent DashStartEvent = new DashStartEvent();
        public static DashEndEvent DashEndEvent = new DashEndEvent();
        public static CounterSuccessEvent CounterSuccessEvent = new CounterSuccessEvent();
    }

    public class AddEXPEvent : GameEvent
    {
        public int exp;
    }
    public class PlayerAttackSuccess : GameEvent { }
    public class DashStartEvent : GameEvent { }
    public class DashEndEvent : GameEvent { }
    public class CounterSuccessEvent : GameEvent
    {
        public Transform target;
    }

}