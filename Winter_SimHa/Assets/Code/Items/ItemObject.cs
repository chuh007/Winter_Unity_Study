﻿using Code.Core.EventSystems;
using UnityEngine;

namespace Code.Items
{
    public class ItemObject : MonoBehaviour, IPickable
    {
        [SerializeField] private Rigidbody2D rbCompo;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private GameEventChannelSO inventoryChannel;
        
        private void OnValidate()
        {
            if (itemData == null) return;
            if (spriteRenderer == null) return;

            spriteRenderer.sprite = itemData.icon;
            gameObject.name = $"ItemObject_[{itemData.itemName}]";
        }

        public void SetItemData(ItemDataSO newData, Vector2 velocity)
        {
            itemData = newData;
            rbCompo.linearVelocity = velocity;
            spriteRenderer.sprite = itemData.icon;
        }
        
        public void PickUp()
        {
            //이부분은 나중에 변경이 되어야 합니다.
            inventoryChannel.RaiseEvent(InventoryEvents.AddItemEvent.Initializer(itemData));
            //나중에 인벤토리로 들어가도록 만들어준다.
            Destroy(gameObject);
        }
    }
}