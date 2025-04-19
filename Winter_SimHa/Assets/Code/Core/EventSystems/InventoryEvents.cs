using System.Collections.Generic;
using System.Threading;
using Code.Items;
using Code.Items.Inven;

namespace Code.Core.EventSystems
{
    public static class InventoryEvents
    {
        public static readonly InventoryDataEvent InventoryDataEvent = new InventoryDataEvent();
        public static readonly RequestInventoryDataEvent RequestInventoryDataEvent = new RequestInventoryDataEvent();
        public static readonly AddItemEvent AddItemEvent = new AddItemEvent();
        public static readonly EquipItemEvent EquipItemEvent = new EquipItemEvent();
        public static readonly UnEquipItemEvent UnEquipItemEvent = new UnEquipItemEvent();
    }

    public class InventoryDataEvent : GameEvent
    {
        public int slotCount;
        public List<InventoryItem> items;
        public Dictionary<EquipType, InventoryItem> equipments;

        public InventoryDataEvent Initializer(int slotCount, List<InventoryItem> items,
            Dictionary<EquipType, InventoryItem> equipments)
        {
            this.slotCount = slotCount;
            this.items = items;
            this.equipments = equipments;
            return this;
        }
    }

    public class RequestInventoryDataEvent : GameEvent { }

    public class AddItemEvent : GameEvent
    {
        public ItemDataSO itemData;

        public AddItemEvent Initializer(ItemDataSO itemData)
        {
            this.itemData = itemData;
            return this;
        }
    }

    public class EquipItemEvent : GameEvent
    {
        public int slotIndex;

        public EquipItemEvent Initializer(int slotIndex)
        {
            this.slotIndex = slotIndex;
            return this;
        }
    }

    public class UnEquipItemEvent : GameEvent
    {
        public EquipType targetType;

        public UnEquipItemEvent Initializer(EquipType targetType)
        {
            this.targetType = targetType;
            return this;
        }
    }
}