using Code.Core.EventSystems;
using Code.Items;
using Code.Players;
using UnityEngine;

namespace Code.UI.MainMenu.InventoryStates
{
    public class InventoryPopupState : UIState
    {
        [SerializeField] private GameEventChannelSO inventoryChannel;
        [SerializeField] private PlayerInputSO playerInput;
        private InventoryPanel _inventoryPanel;

        public override void Initialize(ICanChangeUIState parent)
        {
            base.Initialize(parent);
            _inventoryPanel = (InventoryPanel)parent;
            Debug.Assert(_inventoryPanel != null, "Inventory popup state wrong attached to UI");
        }

        public override void Enter()
        {
            playerInput.OnUIInteractEvent += HandleInteractKey;
            playerInput.OnUICancelEvent += HandleCancelKey;
            
            _inventoryPanel.ItemPopupUI.acceptButton.onClick.AddListener(HandleInteractKey);
            _inventoryPanel.ItemPopupUI.rejectButton.onClick.AddListener(HandleCancelKey);
        }

        public override void Exit()
        {
            playerInput.OnUIInteractEvent -= HandleInteractKey;
            playerInput.OnUICancelEvent -= HandleCancelKey;
            _inventoryPanel.ItemPopupUI.acceptButton.onClick.RemoveListener(HandleInteractKey);
            _inventoryPanel.ItemPopupUI.rejectButton.onClick.RemoveListener(HandleCancelKey);
            _inventoryPanel.HidePopupMenu();
        }

        private void HandleInteractKey()
        {
            if (_inventoryPanel.isOnEquip)
            {
                EquipType type = (EquipType)_inventoryPanel.selectedItemIndex;
                inventoryChannel.RaiseEvent(InventoryEvents.UnEquipItemEvent.Initializer(type));
            }
            else
            {
                //선택된 아이템의 슬롯 번호를 보내준다.
                int slotIndex = _inventoryPanel.selectedItemIndex;
                inventoryChannel.RaiseEvent(InventoryEvents.EquipItemEvent.Initializer(slotIndex));
            }
            
            
            _inventoryPanel.ChangeUIState("NAVIGATE");
        }

        private void HandleCancelKey()
        {
            _inventoryPanel.ChangeUIState("NAVIGATE");
        }
    }
}