using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class EquipmentInventory : MonoBehaviour
    {

        public EquipmentInventorySlot equipmentInventorySlotPrefab;

        public Canvas canvas;
        public Transform pointToDrag;

        public Transform panel;

        private List<EquipmentInventorySlot> _allSlots = new();

        private void Start()
        {

            var equipments = InventoryManager.main.allEquipments;


            for (int i = 0; i < InventoryManager.main.equipments.maxSlot; i++)
            {
                var gb = Instantiate(equipmentInventorySlotPrefab, panel);
                gb.index = i;
                gb.Register(canvas, pointToDrag);
                _allSlots.Add(gb);
            }
            
            
            foreach (var equipment in equipments)
            {
                _allSlots[equipment.index].Refresh(equipment);
            }
            

            // equipments.ForEach(v =>
            // {
            //
            //     var slot = Instantiate(equipmentInventorySlotPrefab, transform);
            //     slot.Refresh(v.item as IEquipment, canvas, pointToDrag);
            // });
        }
        
        
    }
}
