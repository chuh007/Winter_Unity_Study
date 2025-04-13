using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Items.Inven
{
    public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TextMeshProUGUI amountText;

        public RectTransform RectTrm => transform as RectTransform;
        
        public InventoryItem inventoryItem;
        public event Action<int> OnPointerDownEvent;
        private int _slotIndex;
        protected int _slotCount;

        public virtual void Initialize(int slotIndex)
        {
            _slotIndex = slotIndex;
        }
        
        
        public virtual void UpdateSlot(InventoryItem newItem)
        {
            inventoryItem = newItem;
            itemImage.color = Color.white;
            
            if(inventoryItem == null) return;
            
            itemImage.sprite = inventoryItem.data.icon;
            
            if(inventoryItem.stackSize > 1) 
                amountText.text = inventoryItem.stackSize.ToString();
            else
                amountText.text = string.Empty;
        }

        public void CleanUpSlot()
        {
            inventoryItem = null;
            itemImage.color = Color.clear;
            itemImage.sprite = null;
            amountText.text = string.Empty;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
                OnPointerDownEvent?.Invoke(_slotIndex);
        }
    }
}