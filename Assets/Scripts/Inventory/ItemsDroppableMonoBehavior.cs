using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class ItemsDroppableMonoBehavior: MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out EquipmentDragging v)) return;
            
            ItemDidDrop(v);
        }

        protected virtual void ItemDidDrop(EquipmentDragging item)
        {
            
        }
    }
}