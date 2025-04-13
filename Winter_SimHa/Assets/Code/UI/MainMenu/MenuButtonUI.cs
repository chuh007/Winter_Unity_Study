using Code.Core.EventSystems;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MainMenu
{
    public class MenuButtonUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [field: SerializeField] public int UIIndex { get; private set; }
        [field:SerializeField] public MenuUITypeSO TargetUIType { get; private set; }
        [SerializeField] private Image activeImage;

        public void SetSelected(bool isSelected)
        {
            float sizeX = isSelected ? 1f : 0;
            activeImage.transform.DOScaleX(sizeX, 0.2f).SetUpdate(true);
        }
        
        public void ButtonClick()
        {
            ChangeMenuEvent changeEvt = UIEvents.ChangeMenuEvent;
            changeEvt.UIType = TargetUIType;
            uiEventChannel.RaiseEvent(changeEvt);
        }
    }
}