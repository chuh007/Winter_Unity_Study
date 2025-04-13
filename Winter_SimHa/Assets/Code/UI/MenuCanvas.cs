using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.EventSystems;
using Code.Players;
using Code.UI.MainMenu;
using DG.Tweening;
using UnityEngine;

namespace Code.UI
{
    public enum UIWindowStatus
    {
        Closed, Closing, Opening, Opened
    }
    
    public class MenuCanvas : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private RectTransform contentTrm;

        private UIWindowStatus _windowStatus;

        private Dictionary<MenuUITypeSO, MenuPanel> _menuPanels;
        private MenuPanel _currentPanel;
        private Dictionary<MenuUITypeSO, MenuButtonUI> _menuButtons;
        private MenuButtonUI _currentButton;

        private void Awake()
        {
            uiEventChannel.AddListener<OpenMenuEvent>(HandleOpenMenu);
            uiEventChannel.AddListener<ChangeMenuEvent>(HandleChangeMenu);
            
            _menuPanels = new Dictionary<MenuUITypeSO, MenuPanel>();
            contentTrm.GetComponentsInChildren<MenuPanel>().ToList()
                .ForEach(panel => _menuPanels.Add(panel.MenuUITypeSO, panel));
            _menuPanels.Values.ToList().ForEach(panel => panel.Initialize());

            _menuButtons = new Dictionary<MenuUITypeSO, MenuButtonUI>();
            GetComponentsInChildren<MenuButtonUI>().ToList()
                .ForEach(button => _menuButtons.Add(button.TargetUIType, button));
            _menuButtons.Values.ToList().ForEach(button => button.SetSelected(false));
        }

        private void OnDestroy()
        {
            uiEventChannel.RemoveListener<OpenMenuEvent>(HandleOpenMenu);
            uiEventChannel.RemoveListener<ChangeMenuEvent>(HandleChangeMenu);
        }

        private void HandleChangeMenu(ChangeMenuEvent evt)
        {
            OpenPanel(evt.UIType);
        }

        public void OpenPanel(MenuUITypeSO uiType)
        {
            if (_menuPanels.ContainsKey(uiType) == false) return;
            if (_currentPanel == _menuPanels[uiType]) return;
            
            _currentButton?.SetSelected(false);
            _currentButton = _menuButtons[uiType];
            _currentButton.SetSelected(true);
            
            _currentPanel?.Close();
            _currentPanel = _menuPanels[uiType];
            _currentPanel.Open();
            //FSM에서 상태관리를 하듯이 현재 UI패널을 상태처럼 관리한다.
        }

        private void HandleOpenMenu(OpenMenuEvent evt)
        {
            //진행중이라면 무시
            if(_windowStatus == UIWindowStatus.Opening || _windowStatus == UIWindowStatus.Closing) return;

            if (_windowStatus == UIWindowStatus.Closed) //닫혀있으면 열기
            {
                OpenMenuCanvas(evt);
            }else if (_windowStatus == UIWindowStatus.Opened) //열려있으면 닫기
            {
                ClosedMenuCanvas();
            }
        }

        private void OpenMenuCanvas(OpenMenuEvent evt)
        {
            _windowStatus = UIWindowStatus.Opening;
            Time.timeScale = 0;
            playerInput.SetPlayerInput(false); //만들꺼야. 
            SetWindow(true, () => _windowStatus = UIWindowStatus.Opened);
            OpenPanel(evt.UIType);
            playerInput.OnMenuSlideEvent += HandleMenuSlide;
        }

        private void ClosedMenuCanvas()
        {
            _windowStatus = UIWindowStatus.Closing;
            playerInput.OnMenuSlideEvent += HandleMenuSlide;
            playerInput.SetPlayerInput(true);
            SetWindow(false, () =>
            {
                _windowStatus = UIWindowStatus.Closed;
                Time.timeScale = 1f;
                _currentPanel?.Close();
                _currentPanel = null;
            });

        }

        private void HandleMenuSlide(int direction)
        {
            int nextIndex = _currentButton.UIIndex + direction;
            _menuButtons.Values.FirstOrDefault(button => button.UIIndex == nextIndex)?.ButtonClick();
        }

        public void SetWindow(bool isOpen, Action callback = null)
        {
            float alpha = isOpen ? 1f : 0f;
            
            canvasGroup.DOFade(alpha, .3f).SetUpdate(true).OnComplete(() => callback?.Invoke());
            canvasGroup.blocksRaycasts = isOpen;
            canvasGroup.interactable = isOpen;
        }


        [ContextMenu("Toggle Window")]
        private void ToggleWindow()
        {
            bool isOpen = !canvasGroup.blocksRaycasts;
            float alpha = isOpen ? 1f : 0f;
            
            canvasGroup.alpha = alpha;
            canvasGroup.blocksRaycasts = isOpen;
            canvasGroup.interactable = isOpen;
        }
    }
}