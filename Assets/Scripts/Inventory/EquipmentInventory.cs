using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class EquipmentInventory : MonoBehaviour
    {

        public EquipmentInventorySlot equipmentInventorySlotPrefab;

        public Canvas canvas;
        public Transform pointToDrag;

        private void Start()
        {

            var equipments = InventoryManager.main.allEquipments;

            equipments.ForEach(v =>
            {

                var slot = Instantiate(equipmentInventorySlotPrefab, transform);
                slot.Refresh(v, canvas, pointToDrag);
            });
        }
        
        
    }
}
