using Code.Items;
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
            _canMoveSelection = true;
            _panelUI.SetClickEvents(true);
            playerInput.OnUIRightClickEvent += HandleUISubmit;
        }
        
        public override void Exit()
        {
            playerInput.OnUINavigateEvent -= HandleUINavigation;
            playerInput.OnUISubmitEvent -= HandleUISubmit;
            _panelUI.SetClickEvents(false);
            playerInput.OnUIRightClickEvent -= HandleUISubmit;
        }

        private void HandleUINavigation(Vector2 uiMovement)
        {
            if (_canMoveSelection == false) return;
            _canMoveSelection = false;
            
            uiMovement.y *= -1; // y축은 인덱스 방향으로 가면 방향이 반대가 된다.

            if (_panelUI.isOnEquip == false)
            {
                int nextIndex = GetNextSelection(uiMovement);

                if (nextIndex != _panelUI.selectedItemIndex)
                {
                    _panelUI.SelectItem(nextIndex);
                }
                else if (IsOutOfRightBorder(uiMovement.x))
                {
                    _panelUI.isOnEquip = true;
                    _panelUI.SelectItem(EquipType.Weapon); // 무기칸으로
                }
            }
            else
            {
                SetNextEquipSelection(uiMovement);
            }
            

            DOVirtual.DelayedCall(0.01f, () => _canMoveSelection = true).SetUpdate(true);
        }

        // 장비칸 이동 처리
        private void SetNextEquipSelection(Vector2 uiMovement)
        {
            int equipCount = _panelUI.EquipSlotCount; // 전체 갯수
            int nextIndex = _panelUI.selectedItemIndex + Mathf.RoundToInt(uiMovement.x) +
                            2 * Mathf.RoundToInt(uiMovement.y);
            if (nextIndex >= equipCount)
                nextIndex = _panelUI.selectedItemIndex;
            
            if (nextIndex < 0) // 다시 인벤토리 쪽으로
            {
                _panelUI.isOnEquip = false;
                _panelUI.SelectItem(_panelUI.ColCount - 1); // 제일 첫 줄 마지막 아이템
                return;
            }

            _panelUI.SelectItem((EquipType)nextIndex);
        }

        // 다음 이동이 오른쪽을 벗어났냐?
        private bool IsOutOfRightBorder(float uiMovementX)
        {
            int selectedIndex = _panelUI.selectedItemIndex;
            int nextX = selectedIndex % _panelUI.ColCount + Mathf.RoundToInt(uiMovementX);

            return nextX >= _panelUI.ColCount;
        }

        private void HandleUISubmit()
        {
            //선택한 것이 없거나 선택했더라도 빈칸이면 space키를 무시한다.
            if (_panelUI.selectedItem == null || _panelUI.selectedItem.inventoryItem == null) return;

            Vector3 point = _panelUI.RectTrm.InverseTransformPoint(_panelUI.selectedItem.RectTrm.position);

            point.x += _panelUI.selectedItem.RectTrm.sizeDelta.x + 10f;

            if (_panelUI.selectedItem.inventoryItem.data is EquipItemDataSO equipItemData)
            {
                var type = _panelUI.isOnEquip ? ItemPopupUI.PopupType.UnEquip : ItemPopupUI.PopupType.Equip;
                _panelUI.ShowItemMenuPopup(point, type);
                _panelUI.ChangeUIState("POPUP");
            } 
        }
        
        private int GetNextSelection(Vector2 uiMovement)
        {
            if (Mathf.Abs(uiMovement.x) > 0 && Mathf.Abs(uiMovement.y) > 0)
                uiMovement.y = 0;
            
            int selectedItemIndex = _panelUI.selectedItemIndex;
            int currentSlotCount = _panelUI.currentSlotCount;
            int colCount = _panelUI.ColCount;
            
            
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