using System.Collections.Generic;
using UnityEngine;

namespace Tyrant
{
    public class EquipmentInventory : MonoBehaviour
    {

        public EquipmentInventorySlot equipmentInventorySlotPrefab;

        public Transform panel;

        private readonly List<EquipmentInventorySlot> _allSlots = new();


        public Inventory.Type inventoryType;

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
            
            
            // foreach (var equipment in equipments)
            // {
            //     _allSlots[equipment.Value.index].Refresh(equipment.Value);
            // }
        }
        
        
    }
}
