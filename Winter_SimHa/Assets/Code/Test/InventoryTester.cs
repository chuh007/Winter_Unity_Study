using System;
using System.Collections.Generic;
using Code.Items;
using Code.Players;
using UnityEngine;

namespace Code.Test
{
    public class InventoryTester : MonoBehaviour
    {
        [SerializeField] private PlayerInvenData invenData;
        public bool isTest;
        public List<ItemDataSO> initialInventory;

        private void Start()
        {
            if(isTest == false) return;

            foreach (var item in initialInventory)
            {
                invenData.AddItem(item);
            }
        }
    }
}