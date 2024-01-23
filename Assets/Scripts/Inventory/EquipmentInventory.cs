using System;
using System.Collections.Generic;
using Tyrant.UI;
using UnityEngine;

namespace Tyrant
{
    public class EquipmentInventory : MonoBehaviour
    {

        public EquipmentInventorySlot equipmentInventorySlotPrefab;

        public Transform panel;

        private readonly List<EquipmentInventorySlot> _allSlots = new();

        public ItemInspectorMessageChannel messageChannel;

        public Inventory.Type inventoryType;

        public ItemInspector itemInspector;
        
        private void Start()
        {

            var all = InventoryManager.main.InventoryBy(inventoryType);


            for (int i = 0; i < all.maxSlot; i++)
            {
                var gb = Instantiate(equipmentInventorySlotPrefab, panel);
                gb.inventoryType = inventoryType;
                gb.index = i;
                _allSlots.Add(gb);
            }

            if (itemInspector != null)
            {
                itemInspector.gameObject.SetActive(false);
            }
            
        }

        private void OnEnable()
        {
            messageChannel.itemInspector += ItemInspector;
        }

        private void OnDisable()
        {
            messageChannel.itemInspector -= ItemInspector;
        }

        private void ItemInspector(IItem arg0)
        {
            if (itemInspector == null) return;
            itemInspector.gameObject.SetActive(arg0 != null);
            itemInspector.NewItem(arg0);
        }
    }
}
