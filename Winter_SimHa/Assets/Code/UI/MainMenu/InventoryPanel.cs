using System.Collections.Generic;
using System.Linq;
using Code.Core.EventSystems;
using Code.Items;
using Code.Items.Inven;
using Code.Players;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.UI.MainMenu
{
    public class InventoryPanel : MenuPanel, ICanChangeUIState
    {
        [SerializeField] private GameEventChannelSO inventoryChannel;
        [SerializeField] private ItemSelectionUI itemSelectionUI;
        [SerializeField] private ItemPopupUI itemPopupUI;
        [field: SerializeField] public int ColCount { get; private set; } = 8;

        public List<InventoryItem> inventory;

        [SerializeField] private Transform slotParentTrm;
        private ItemSlotUI[] _itemSlots;
        
        [HideInInspector] public int currentSlotCount;
        [HideInInspector] public ItemSlotUI selectedItem;
        [HideInInspector] public int selectedItemIndex;
        private bool canMoveSelection;

        public UnityEvent<ItemDataSO> OnItemSelected;

        private UIState _currentUIState;
        private Dictionary<string, UIState> _states = new Dictionary<string, UIState>();
        
        protected virtual void Awake()
        {
            _itemSlots = slotParentTrm.GetComponentsInChildren<ItemSlotUI>(); //슬롯들을 다 가져와서 저장
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                _itemSlots[i].Initialize(i);
                _itemSlots[i].OnPointerDownEvent += SelectItem;
            }
            
            GetComponentsInChildren<UIState>().ToList().ForEach(state =>
            {
                state.Initialize(this);
                _states.Add(state.name, state);
            });
        }

        public override void Open()
        {
            base.Open();
            inventoryChannel.AddListener<InventoryDataEvent>(HandleDataRefresh); //데이터 오는걸 구독
            inventoryChannel.RaiseEvent(InventoryEvents.RequestInventoryDataEvent); //데이터 요청 이벤트 발행
            
            SelectItem(0);
            ChangeUIState("NAVIGATE");
            HidePopupMenu();
        }


        public override void Close()
        {
            base.Close();
            inventoryChannel.RemoveListener<InventoryDataEvent>(HandleDataRefresh); //데이터 오는걸 구독
            _currentUIState.Exit();
        }

        public void SelectItem(int nextIndex)
        {
            selectedItem = _itemSlots[nextIndex];
            selectedItemIndex = nextIndex;
            itemSelectionUI.MoveAnchorPosition(selectedItem.RectTrm.anchoredPosition);
            OnItemSelected?.Invoke(selectedItem.inventoryItem?.data);
        }
        
        private void HandleDataRefresh(InventoryDataEvent evt)
        {
            inventory = evt.items; //받아온 아이템 갱신후
            currentSlotCount = evt.slotCount;
            UpdateSlotUI();
        }

        private void UpdateSlotUI()
        {
            for (int i = 0; i < currentSlotCount; i++)
            {
                _itemSlots[i].gameObject.SetActive(true);
                _itemSlots[i].CleanUpSlot();
            }

            for (int i = currentSlotCount; i < _itemSlots.Length; i++)
                _itemSlots[i].gameObject.SetActive(false);
            
            for (int i = 0; i < inventory.Count; i++)
                _itemSlots[i].UpdateSlot(inventory[i]);
        }


        public void ChangeUIState(string stateName)
        {
            _currentUIState.Exit();
            _currentUIState = _states.GetValueOrDefault(stateName);
            Debug.Assert(_currentUIState != null);
            _currentUIState.Enter();
        }
        #region Popup Controll

        
        private void HidePopupMenu()
        {
            itemPopupUI.SetActiveUI(false);
        }

        public void ShowItemMenuPopup(Vector2 point)
        {
            itemPopupUI.ShowPopupUI(point);
        }
        
        #endregion
    }
}
