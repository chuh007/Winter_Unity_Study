using System;
using System.Collections.Generic;
using Code.Items;
using UnityEngine;

namespace Code.Core.GameSystem
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "SO/Items/Database", order = 0)]
    public class ItemDatabaseSO : ScriptableObject
    {
        public List<ScrapItemDataSO> scrapItems;
        public List<EquipItemDataSO> equipItems;
        
        private Dictionary<string, ItemDataSO> _itemDatabase = new Dictionary<string, ItemDataSO>();

        public ItemDataSO GetItem(string itemId)
            => _itemDatabase.GetValueOrDefault(itemId);

        public void ClearAllItems()
        {
            scrapItems?.Clear();
            equipItems?.Clear();  
        }
        private void OnEnable()
        {
            if (scrapItems != null)
            {
                foreach (var scrapItem in scrapItems)
                {
                    _itemDatabase.Add(scrapItem.itemID, scrapItem);
                }
            }

            if (equipItems != null)
            {
                foreach (var equipItem in equipItems)
                {
                    _itemDatabase.Add(equipItem.itemID, equipItem);
                }
            }
        }
    }
}