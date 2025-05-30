﻿using System.Collections.Generic;
using System.Linq;
using Code.Core.EventSystems;
using Code.Items;
using Code.Items.Inven;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI.MainMenu
{
    public class InventoryPanel : MenuPanel, ICanChangeUIState
    {
        [SerializeField] private GameEventChannelSO inventoryChannel;
        [SerializeField] private ItemSelectionUI itemSelectionUI;
        
        [field:SerializeField] public int ColCount { get; private set; }

        //실제 인벤데이터를 참조하고 있는 구조
        #region Read data storage
        public List<InventoryItem> inventory;
        public Dictionary<EquipType,InventoryItem> equipments;
        #endregion

        //UI를 참조하고 있는 구조
        #region UI References 
        [field: SerializeField] public ItemPopupUI ItemPopupUI { get; private set; }
        [SerializeField] private Transform slotParentTrm, equipSlotParentTrm;
        private ItemSlotUI[] _itemSlots;
        private Dictionary<EquipType, EquipSlotUI> _equipSlots;
        public int EquipSlotCount => _equipSlots.Count; //장비창의 갯수
        #endregion

        [HideInInspector] public bool isOnEquip; //장비창에 가 있는가?
        [HideInInspector] public int currentSlotCount;
        [HideInInspector] public ItemSlotUI selectedItem;
        [HideInInspector] public int selectedItemIndex;
        
        public UnityEvent<ItemDataSO> OnItemSelected;
        
        private UIState _currentUIState;
        private Dictionary<string, UIState> _states = new Dictionary<string, UIState>();
        
        protected virtual void Awake()
        {
            _itemSlots = slotParentTrm.GetComponentsInChildren<ItemSlotUI>(); //슬롯들을 다 가져와서 저장
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                _itemSlots[i].Initialize(i); //슬롯 인덱스 부여해주고
                //_itemSlots[i].OnPointerDownEvent += SelectItem;
            }
            
            _equipSlots = new Dictionary<EquipType, EquipSlotUI>();
            equipSlotParentTrm.GetComponentsInChildren<EquipSlotUI>().ToList()
                .ForEach(slot =>
                {
                    slot.Initialize((int)slot.slotType);
                    _equipSlots.Add(slot.slotType, slot);
                });
            
            GetComponentsInChildren<UIState>().ToList().ForEach(state =>
            {
                state.Initialize(this);
                _states.Add(state.name, state); //게임오브젝트의 이름으로 상태를 정한다.
            });
        }

        public override void Open()
        {
            base.Open();
            inventoryChannel.AddListener<InventoryDataEvent>(HandleDataRefresh); //데이터 오는걸 구독
            inventoryChannel.RaiseEvent(InventoryEvents.RequestInventoryDataEvent); //데이터 요청 이벤트 발행
            
            SelectItem(0); //맨 처음 열었을 때 첫번째 아이템 선택하게.
            ChangeUIState("NAVIGATE");
            HidePopupMenu();
        }

        public override void Close()
        {
            base.Close();
            inventoryChannel.RemoveListener<InventoryDataEvent>(HandleDataRefresh); //데이터 오는걸 구독
            _currentUIState?.Exit();
        }
        
        public void SelectItem(int nextIndex)
        {
            isOnEquip = false;
            selectedItem = _itemSlots[nextIndex];
            selectedItemIndex = nextIndex;
            MoveSelectionUI();
        }
        
        private void SelectEquipItem(int equipIndex)
        {
            //여기서 해줄 일을 생각해봐라. 그에 따라 기존 SelectItem도 살짝 변경이 일어나야 한다.
            isOnEquip = true;
            SelectItem((EquipType) equipIndex);
        }

        public void SelectItem(EquipType equipType)
        {
            selectedItem = _equipSlots[equipType];
            selectedItemIndex = (int) equipType;
            MoveSelectionUI();
        }

        private void MoveSelectionUI()
        {
            Vector2 anchorPoint = RectTrm.InverseTransformPoint(selectedItem.RectTrm.position);
            itemSelectionUI.MoveAnchorPosition(anchorPoint, true);
            OnItemSelected?.Invoke(selectedItem.inventoryItem?.data);
        }

        private void HandleDataRefresh(InventoryDataEvent evt)
        {
            inventory = evt.items; //받아온 아이템 갱신후
            equipments = evt.equipments; //장비아이템도 갱신
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
            for(int i = currentSlotCount; i < _itemSlots.Length; i++)
                _itemSlots[i].gameObject.SetActive(false);

            for (int i = 0; i < inventory.Count; i++)
                _itemSlots[i].UpdateSlot(inventory[i]);

            foreach (EquipSlotUI slot in _equipSlots.Values)
            {
                slot.CleanUpSlot();
            }
            
            foreach (KeyValuePair<EquipType, InventoryItem> equipKvp in equipments)
            {
                _equipSlots[equipKvp.Key].UpdateSlot(equipKvp.Value);
            }
        }

        public void ChangeUIState(string stateName)
        {
            _currentUIState?.Exit();
            _currentUIState = _states.GetValueOrDefault(stateName);
            //시작하기전 여기 바꾸세요. ==  => != 으로 (지난시간에 오타남)
            Debug.Assert(_currentUIState != default, $"UI stat is null check : {stateName}");
            _currentUIState.Enter();
        }

        public void SetClickEvents(bool isActive)
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if(isActive) 
                    _itemSlots[i].OnPointerDownEvent += SelectItem;
                else
                    _itemSlots[i].OnPointerDownEvent -= SelectItem;
            }

            foreach (EquipSlotUI slot in _equipSlots.Values)
            {
                if(isActive)
                    slot.OnPointerDownEvent += SelectEquipItem;
                else
                    slot.OnPointerDownEvent -= SelectEquipItem;
            }
        }

        
        

        #region Popup Control

        public void HidePopupMenu()
        {
            ItemPopupUI.SetActiveUI(false);
        }

        public void ShowItemMenuPopup(Vector2 point, ItemPopupUI.PopupType popupType)
        {
            ItemPopupUI.ShowPopupUI(point, popupType);
        }

        #endregion
        
    }
}