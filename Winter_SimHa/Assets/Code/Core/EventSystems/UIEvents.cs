using Code.UI.MainMenu;

namespace Code.Core.EventSystems
{
    public static class UIEvents
    {
        public static FadeEvent FadeEvent = new FadeEvent();
        public static FadeCompleteEvent FadeCompleteEvent = new FadeCompleteEvent();
        public static OpenMenuEvent OpenMenuEvent = new OpenMenuEvent();
        public static ChangeMenuEvent ChangeMenuEvent = new ChangeMenuEvent();
    }

    public class FadeEvent : GameEvent
    {
        public bool isFadeIn;
        public float fadeTime;
        public bool isSaveOrLoad; //저장이나 로딩을 하는거냐?
    }

    public class FadeCompleteEvent : GameEvent { }

    public class OpenMenuEvent : GameEvent
    {
        public MenuUITypeSO UIType;
    }

    public class ChangeMenuEvent : GameEvent
    {
        public MenuUITypeSO UIType;
    }
}