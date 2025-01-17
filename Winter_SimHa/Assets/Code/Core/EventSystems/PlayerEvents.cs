namespace Code.Core.EventSystems
{
    public static class PlayerEvents
    {
        public static AddEXPEvent AddExpEvent = new AddEXPEvent();
    }

    public class AddEXPEvent : GameEvent
    {
        public int exp;
    }
}