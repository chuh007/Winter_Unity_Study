using Code.Players;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.MainMenu.InventoryStates
{
    public class InventoryNavigateState : UIState
    {
        [SerializeField] private PlayerInputSO playerInput;

        private InventoryPanel _panelUI;
        private bool _canMoveSelection;
        
        public override void Initialize(ICanChangeUIState parent)
        {
            base.Initialize(parent);
            _panelUI = (InventoryPanel) parent;
            Debug.Assert(_panelUI != null, "Inventory navigation state wrong attached to UI");
        }

        public override void Enter()
        {
            playerInput.OnUINavigateEvent += HandleUINavigation;
            playerInput.OnUISubmitEvent += HandleUISubmit;
        }
        
        public override void Exit()
        {
            playerInput.OnUINavigateEvent -= HandleUINavigation;
            playerInput.OnUISubmitEvent -= HandleUISubmit;
        }

        private void HandleUINavigation(Vector2 uiMovement)
        {
            if (_canMoveSelection == false) return;
            _canMoveSelection = false;
            
            int nextIndex = GetNextSelection(uiMovement);
            
            if(nextIndex != _panelUI.selectedItemIndex)
                _panelUI.SelectItem(nextIndex);

            DOVirtual.DelayedCall(0.01f, () => _canMoveSelection = true).SetUpdate(true);
        }
        
        private void HandleUISubmit()
        {
            //선택한 것이 없거나 선택했더라도 빈칸이면 space키를 무시한다.
            if (_panelUI.selectedItem == null || _panelUI.selectedItem.inventoryItem == null) return;

            Vector3 point = _panelUI.RectTrm.InverseTransformPoint(_panelUI.selectedItem.RectTrm.position);

            point.x += _panelUI.selectedItem.RectTrm.sizeDelta.x + 10f;

            _panelUI.ShowItemMenuPopup(point);
            _panelUI.ChangeUIState("POPUP");
        }
        
        private int GetNextSelection(Vector2 uiMovement)
        {
            if (Mathf.Abs(uiMovement.x) > 0 && Mathf.Abs(uiMovement.y) > 0)
                uiMovement.y = 0;
            
            int selectedItemIndex = _panelUI.selectedItemIndex;
            int currentSlotCount = _panelUI.currentSlotCount;
            int colCount = _panelUI.ColCount;
            
            uiMovement.y *= -1; // y축은 인덱스 방향으로 가면 방향이 반대가 된다.
            Vector2Int currentPosition = new Vector2Int(selectedItemIndex % colCount, selectedItemIndex / colCount);
            int totalRows = Mathf.CeilToInt((float)currentSlotCount / colCount );
            currentPosition += Vector2Int.RoundToInt(uiMovement);

            if (currentPosition.x >= colCount || currentPosition.x < 0
                                              || currentPosition.y < 0 || currentPosition.y >= totalRows)
                return selectedItemIndex; //기존 인덱스를 그대로 리턴
            
            int nextIndex = currentPosition.x + currentPosition.y * colCount;
            return nextIndex < currentSlotCount ? nextIndex : selectedItemIndex;
        }
    }
}