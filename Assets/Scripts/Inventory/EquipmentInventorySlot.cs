using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class EquipmentInventorySlot: MonoBehaviour, IDropHandler
    {
        public EquipmentDragging equipmentDraggingPrefab;

        public int index;
        
        public void Refresh(Inventory.Slot e)
        {
            Instantiate(equipmentDraggingPrefab, transform).equipment = e;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out EquipmentDragging v)) return;
            
            var item = v.equipment.item;
            v.DidRemoveItem();
            Destroy(v.gameObject);

            var slot = new Inventory.Slot(index, item);
                
            InventoryManager.main.AddSlot(slot);
                
            Refresh(slot);
        }
    }
}