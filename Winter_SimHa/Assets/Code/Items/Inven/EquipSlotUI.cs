using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Items.Inven
{
    public class EquipSlotUI : ItemSlotUI
    {
        public EquipType slotType;
        [SerializeField] private Image backgroundImage;
        
        private Color _backgroundColor;

        private void OnValidate()
        {
            gameObject.name = $"EquipSlotUI_{slotType}";
        }

        public override void Initialize(int slotIndex)
        {
            base.Initialize(slotIndex);
            _backgroundColor = backgroundImage.color;
        }

        public override void UpdateSlot(InventoryItem newItem)
        {
            base.UpdateSlot(newItem);

            backgroundImage.color = inventoryItem != null ? Color.clear : _backgroundColor;
        }

        public override void CleanUpSlot()
        {
            base.CleanUpSlot();
            backgroundImage.color = _backgroundColor;
        }
    }
}