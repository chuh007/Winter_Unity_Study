using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.EventSystems;
using Code.Entities;
using Code.Items;
using Code.Items.Inven;
using UnityEngine;

namespace Code.Players
{
    public class PlayerInvenData : InvenData, IEntityComponent, IAfterInit
    {
        [SerializeField] private GameEventChannelSO inventoryChannel;
        [SerializeField] private int maxSlotCount = 14;
        private Player _player;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            inventory = new List<InventoryItem>();
        }

        public void AfterInit()
        {
            inventoryChannel.AddListener<RequestInventoryDataEvent>(HandleRequestInventoryData);
            inventoryChannel.AddListener<AddItemEvnet>(HandleAddItem);
        }

        private void OnDestroy()
        {
            inventoryChannel.RemoveListener<RequestInventoryDataEvent>(HandleRequestInventoryData);
            inventoryChannel.RemoveListener<AddItemEvnet>(HandleAddItem);
        }

        private void HandleAddItem(AddItemEvnet evt) => AddItem(evt.itemData);

        private void HandleRequestInventoryData(RequestInventoryDataEvent evt)
        {
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            inventoryChannel.RaiseEvent(
                InventoryEvents.InventoryDataEvent.Initializer(maxSlotCount,inventory));
        }

        public override void AddItem(ItemDataSO itemData, int count = 1)
        {
            IEnumerable<InventoryItem> items = GetItems(itemData);
            InventoryItem canAddItem = items.FirstOrDefault(item => item.IsFullStack == false);

            if (canAddItem == default)
            {
                CreateNewInventory(itemData, count);
            }
            else
            {
                int remain = canAddItem.AddStack(count);
                if(remain > 0)
                    CreateNewInventory(itemData, remain);
            }
            UpdateInventoryUI();
        }

        private void CreateNewInventory(ItemDataSO itemData,  int count)
        {
            InventoryItem newItem = new InventoryItem(itemData,count);
            inventory.Add(newItem);
        }

        public override void RemoveItem(ItemDataSO itemData, int count)
        {
            UpdateInventoryUI();
        }

        public override bool CanAddItem(ItemDataSO itemData)
        {
            if (inventory.Count < maxSlotCount - 1) return true;
            
            IEnumerable<InventoryItem> items = GetItems(itemData);
            InventoryItem canAddItem = items.FirstOrDefault(item => item.IsFullStack == false);
            return canAddItem != default;
        }

        
    }
}