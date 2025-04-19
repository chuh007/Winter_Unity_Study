using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MainMenu
{
    public class ItemPopupUI : MonoBehaviour
    {
        public enum PopupType
        {
            Equip = 0, UnEquip = 1
        }

        [SerializeField] private TextDataBaseSO textDB;
        public Button acceptButton, rejectButton;
        
        private RectTransform _rectTrm;
        private CanvasGroup _canvasGroup;

        private TextMeshProUGUI _acceptText;
        private TextMeshProUGUI _rejectText;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _acceptText = acceptButton.GetComponentInChildren<TextMeshProUGUI>();
            _rejectText = rejectButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetActiveUI(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1f : 0f;
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }

        public void ShowPopupUI(Vector2 point, PopupType popupType)
        {
            SetActiveUI(true);
            _acceptText.text = textDB[popupType.ToString()];
            _rejectText.text = textDB["Cancel"]; // TODO TextDB 만들어 가져옴
            
            _rectTrm.anchoredPosition = point;
        }
    }
}