using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class InventoryDestroy: MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent(out EquipmentDragging drag))
            {
                var r = drag.equipment;

                InventoryManager.main.Remove(r);
                
                Destroy(drag.gameObject);

            }
        }
    }
}