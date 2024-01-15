using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class ForgeItemRequiresSlot: ItemsDroppableMonoBehavior
    {
        
        public EquipmentDragging existsDragging;

        public UnityAction<IItem> itemDropped;

        public bool hasValue
        {
            get
            {
                if (existsDragging == null) return false;
                var value = existsDragging.equipment.item;
                return value != null;
            }
        }

        public IItem itemValue => existsDragging.equipment.item;
        protected override void ItemDidDrop(EquipmentDragging item)
        {
            if (hasValue)
            {
                existsDragging.UnsetTemporaryPin();
                existsDragging = null;
            }
            
            item.TemporaryPin();
            
            item.transform.SetParent(transform);

            existsDragging = item;
            
            itemDropped?.Invoke(itemValue);
        }
        
    }
}