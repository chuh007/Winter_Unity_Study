using System;
using DG.Tweening;
using UnityEngine;

namespace Code.UI
{
    public class ItemSelectionUI : MonoBehaviour
    {
        [SerializeField] private float tweenDuration;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        public void MoveAnchorPosition(Vector2 anchorPosition, bool isTween = false)
        {
            if (isTween)
            {
                _rectTransform.DOKill();
                _rectTransform.DOAnchorPos(anchorPosition, tweenDuration).SetUpdate(true);
            }
            else
            {
                _rectTransform.anchoredPosition = anchorPosition;
            }
        }
    }
}