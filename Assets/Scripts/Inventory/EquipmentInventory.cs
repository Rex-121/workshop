using System.Collections.Generic;
using UnityEngine;

namespace Tyrant
{
    public class EquipmentInventory : MonoBehaviour
    {

        public EquipmentInventorySlot equipmentInventorySlotPrefab;

        public Transform panel;

        private readonly List<EquipmentInventorySlot> _allSlots = new();

        private void Start()
        {

            var equipments = InventoryManager.main.allEquipments;


            for (int i = 0; i < InventoryManager.main.equipments.maxSlot; i++)
            {
                var gb = Instantiate(equipmentInventorySlotPrefab, panel);
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
