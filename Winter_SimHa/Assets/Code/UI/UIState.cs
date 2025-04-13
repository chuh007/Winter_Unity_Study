using UnityEngine;

namespace Code.UI
{
    public abstract class UIState : MonoBehaviour
    {
        protected ICanChangeUIState _parent;

        public virtual void Initialize(ICanChangeUIState parent)
        {
            _parent = parent;
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}