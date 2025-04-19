using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.EventSystems;
using Code.Core.GameSystem;
using Code.Entities;
using Code.Items;
using Code.Items.Inven;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.Players
{
    public class PlayerInvenData : InvenData, IEntityComponent, IAfterInit, ISavable
    {
        [SerializeField] private GameEventChannelSO inventoryChannel;
        [SerializeField] private int maxSlotCount = 14;
        private Player _player;

        private Dictionary<EquipType, InventoryItem> _equipSlots;
        private EntityStat _statCompo;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            inventory = new List<InventoryItem>(); //인벤토리에 빈칸 만들어주고
            
            _equipSlots = new Dictionary<EquipType, InventoryItem>();
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInit()
        {
            inventoryChannel.AddListener<RequestInventoryDataEvent>(HandleRequestPlayerInvenData);
            inventoryChannel.AddListener<AddItemEvent>(HandleAddItem);
            inventoryChannel.AddListener<EquipItemEvent>(HandleEquipItem);
            inventoryChannel.AddListener<UnEquipItemEvent>(HandleUnEquipItem);
        }

        private void OnDestroy()
        {
            inventoryChannel.RemoveListener<RequestInventoryDataEvent>(HandleRequestPlayerInvenData);
            inventoryChannel.RemoveListener<AddItemEvent>(HandleAddItem);
            inventoryChannel.RemoveListener<EquipItemEvent>(HandleEquipItem);
            inventoryChannel.RemoveListener<UnEquipItemEvent>(HandleUnEquipItem);
        }

        private void HandleUnEquipItem(UnEquipItemEvent evt)
        {
            EquipType equipType = evt.targetType;
            EquipItemDataSO targetItem = _equipSlots[equipType].data as EquipItemDataSO;
            
            Debug.Assert(targetItem != null, "Target item is null, check code error");
            if (CanAddItem(targetItem))
            {
                _equipSlots.Remove(equipType);
                targetItem.RemoveModifier(_statCompo); //해제한 경우 스탯 초기화
                AddItem(targetItem); //인벤토리에 장착해제한 아이템 넣기
            }
            else
            {
                Debug.LogWarning("Not enough space for unequip item");                
            }

        }

        private void HandleEquipItem(EquipItemEvent evt)
        {
            int slotIndex = evt.slotIndex;
            if (slotIndex >= inventory.Count) return; //무시한다.
            
            InventoryItem item = inventory[slotIndex];

            if (item.data is EquipItemDataSO equipItemData)
            {
                InventoryItem beforeEquipItem = _equipSlots.GetValueOrDefault(equipItemData.equipType); 
                //이전에 해당 칸에 장비하고 있던 것

                if (beforeEquipItem != default)
                {
                    _equipSlots[equipItemData.equipType] = item;
                    inventory[slotIndex] = beforeEquipItem;
                    EquipItemDataSO beforeEquipData = beforeEquipItem.data as EquipItemDataSO;
                    beforeEquipData?.RemoveModifier(_statCompo); //장비를 해제했으니 해당 장비의 능력치를 해제
                }
                else
                {
                    _equipSlots.Add(equipItemData.equipType, item);
                    inventory.RemoveAt(slotIndex); //해당 슬롯번호를 제거
                }
                equipItemData.AddModifier(_statCompo); //새로 장착한 녀석의 능력치를 적용
                UpdateInventoryUI(); //이에 따라 업데이트
            }
        }

        private void HandleAddItem(AddItemEvent evt) => AddItem(evt.itemData);
        private void HandleRequestPlayerInvenData(RequestInventoryDataEvent evt)
        {
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            inventoryChannel.RaiseEvent(
                InventoryEvents.InventoryDataEvent.Initializer(maxSlotCount, inventory, _equipSlots));
        }

        public override void AddItem(ItemDataSO itemData, int count = 1)
        {
            IEnumerable<InventoryItem> items = GetItems(itemData);
            InventoryItem canAddItem = items.FirstOrDefault(item => item.IsFullStack == false);

            if (canAddItem == default)
            {
                CreateNewInventory(itemData, count); //풀스택이 false인 칸이 없다면 새로 칸을 만들어서 넣는다.
            }
            else
            {
                int remain = canAddItem.AddStack(count); //주운만큼 넣어지고 남는건 반환
                if(remain > 0)
                    CreateNewInventory(itemData, remain);
            }
            
            UpdateInventoryUI();
        }

        private void CreateNewInventory(ItemDataSO itemData, int count)
        {
            InventoryItem newItem = new InventoryItem(itemData, count);
            inventory.Add(newItem);
        }

        public override void RemoveItem(ItemDataSO itemData, int count)
        {
            UpdateInventoryUI(); //제거는 나중에 만들고 나서
        }

        public override bool CanAddItem(ItemDataSO itemData)
        {
            if (inventory.Count < maxSlotCount - 1) return true;
            
            IEnumerable<InventoryItem> items = GetItems(itemData);
            InventoryItem canAddItem = items.FirstOrDefault(item => item.IsFullStack == false);
            
            return canAddItem != default; 
        }


        
        #if UNITY_EDITOR
        [ContextMenu("Load all item data")]
        private void LoadAllItemData()
        {
            if (itemDB == null)
            {
                Debug.LogWarning("No item DB found");
                return;
            }

            string path = "Assets/08SO/ItemData";
            string[] assetNames = AssetDatabase.FindAssets("", new[] {path});
            itemDB.ClearAllItems();
            foreach (string guid in assetNames)
            {
                string soPath = AssetDatabase.GUIDToAssetPath(guid);
                ItemDataSO itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>(soPath);
                if (itemData != null)
                {
                    if(itemData is ScrapItemDataSO scrapItemDataSO)
                        itemDB.scrapItems.Add(scrapItemDataSO);
                    else if(itemData is EquipItemDataSO equipItemDataSO)
                        itemDB.equipItems.Add(equipItemDataSO);
                }
            }
            
            EditorUtility.SetDirty(itemDB);
            AssetDatabase.SaveAssets();
        }
        #endif
        
        #region SaveLogic

        [SerializeField] private ItemDatabaseSO itemDB;
        [field: SerializeField] public SaveIdSO SaveID { get; private set; }

        [Serializable]
        public struct InvenItemSaveData
        {
            public string itemId;
            public int stackSize;
            public int slotIndex;
        }

        [Serializable]
        public struct EquipItemSaveData
        {
            public EquipType equipType;
            public string itemId;
        }
        
        [Serializable]
        public struct InvenSaveData
        {
            public List<InvenItemSaveData> items;
            public List<EquipItemSaveData> equipments;
        }
        
        public string GetSaveData()
        {
            InvenSaveData saveData;
            saveData.items = inventory.Select((item, idx) => new InvenItemSaveData
            {
                itemId = item.data.itemID,
                stackSize = item.stackSize,
                slotIndex = idx
            }).ToList();

            saveData.equipments = _equipSlots.Select(kvp => new EquipItemSaveData
            {
                equipType = kvp.Key,
                itemId = kvp.Value.data.itemID
            }).ToList();
            
            return JsonUtility.ToJson(saveData);
        }

        public void RestoreData(string loadedData)
        {
            InvenSaveData loadedSaveData = JsonUtility.FromJson<InvenSaveData>(loadedData);
            loadedSaveData.items.Sort((item1, item2) => item1.slotIndex - item2.slotIndex);
            
            inventory = loadedSaveData.items.Select(saveItem =>
            {
                ItemDataSO itemData = itemDB.GetItem(saveItem.itemId);
                Debug.Assert(itemData != null, $"Save data corrupted : {saveItem.itemId} is not exist on DB");
                return new InventoryItem(itemData, saveItem.stackSize);
            }).ToList();
            _equipSlots.Clear();
            loadedSaveData.equipments.ForEach(saveEquipment =>
            {
                ItemDataSO itemData = itemDB.GetItem(saveEquipment.itemId);
                Debug.Assert(itemData != null, $"Save data corrupted : {saveEquipment.itemId} is not exist on DB");
                if (itemData is EquipItemDataSO equipItemDataSO)
                {
                    equipItemDataSO.AddModifier(_statCompo);
                    _equipSlots.Add(equipItemDataSO.equipType, new InventoryItem(equipItemDataSO));
                }
            });
            
        }

        #endregion
        
    }
}