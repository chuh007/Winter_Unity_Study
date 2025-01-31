using Code.Core.EventSystems;
using Code.Players;
using Code.UI.MainMenu;
using UnityEngine;

namespace Code.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [SerializeField] private MenuUITypeSO defaultUIType;

        private void Awake()
        {
            playerInput.OnOpenMenuKeyPressed += HandleOpenMenuKeyPressed;
        }

        private void OnDestroy()
        {
            playerInput.OnOpenMenuKeyPressed -= HandleOpenMenuKeyPressed;
        }

        private void HandleOpenMenuKeyPressed()
        {
            OpenMenuEvent openEvt = UIEvents.OpenMenuEvent;
            openEvt.UIType = defaultUIType;
            
            uiEventChannel.RaiseEvent(openEvt);
        }
    }
}