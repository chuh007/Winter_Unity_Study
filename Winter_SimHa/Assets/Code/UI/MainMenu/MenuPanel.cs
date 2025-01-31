using DG.Tweening;
using UnityEngine;

namespace Code.UI.MainMenu
{
    public abstract class MenuPanel : MonoBehaviour
    {
        [field: SerializeField] public MenuUITypeSO MenuUITypeSO { get; protected set; }
        [SerializeField] protected float parentHeight = 800f;
        
        protected RectTransform _rectTrm;
        protected Vector2 _initPosition;

        public virtual void Initialize()
        {
            _rectTrm = transform as RectTransform;
            Debug.Assert(_rectTrm != null, "Cannot be null : RectTransform");
            
            _initPosition = _rectTrm.anchoredPosition;
            _rectTrm.anchoredPosition = _initPosition + new Vector2(0f, - parentHeight);
        }

        public virtual void Open()
        {
            _rectTrm.DOAnchorPos(_initPosition, 0.5f).SetUpdate(true);
        }

        public virtual void Close()
        {
            Vector2 hidePosition = _initPosition + new Vector2(0f, -parentHeight);
            _rectTrm.DOAnchorPos(hidePosition, 0.5f).SetUpdate(true);
        }
        
    }
}