using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Items.Inven
{
    public abstract class InvenData : MonoBehaviour
    {
        public List<InventoryItem> inventory; 
        // 아이템 데이터(SO)와 갯수를 가진 인벤토리 한 칸의 데이터
        
        public virtual InventoryItem GetItem(ItemDataSO itemData) 
            => inventory.FirstOrDefault(invenItem => invenItem.data == itemData);

        public virtual IEnumerable<InventoryItem> GetItems(ItemDataSO itemData)
            => inventory.Where(invenItem => invenItem.data == itemData);
        
        public abstract void AddItem(ItemDataSO itemData, int count = 1);
        public abstract void RemoveItem(ItemDataSO itemData, int count);
        public abstract bool CanAddItem(ItemDataSO itemData);
    }
}