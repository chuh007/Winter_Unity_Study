using System.Collections.Generic;
using Code.Items;
using Code.Items.Inven;

namespace Code.Core.EventSystems
{
    public class InventoryEvents
    {
        public static InventoryDataEvent InventoryDataEvent = new InventoryDataEvent();
        public static RequestInventoryDataEvent RequestInventoryDataEvent = new RequestInventoryDataEvent();
        public static AddItemEvnet AddItemEvnet = new AddItemEvnet();
    }

    public class InventoryDataEvent : GameEvent
    {
        public int slotCount;
        public List<InventoryItem> items;

        public InventoryDataEvent Initializer(int slotCount, List<InventoryItem> items)
        {
            this.slotCount = slotCount;
            this.items = items;
            return this;
        }
    }

    public class RequestInventoryDataEvent : GameEvent
    {
        
    }

    public class AddItemEvnet : GameEvent
    {
        public ItemDataSO itemData;

        public AddItemEvnet Initializer(ItemDataSO itemData)
        {
            this.itemData = itemData;
            return this;
        }
    }
    
}