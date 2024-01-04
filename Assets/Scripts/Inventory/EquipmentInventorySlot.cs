using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentInventorySlot: MonoBehaviour, IDropHandler
    {


        
        public EquipmentDragging equipmentDraggingPrefab;

        public int index;
        
        private Inventory.Slot _equipment;

        [ShowInInspector]
        private Canvas _canvas;
        [ShowInInspector]
        private Transform _pointToDrag;

        public void Register(Canvas canvas, Transform pointToDrag)
        {
            _canvas = canvas;
            _pointToDrag = pointToDrag;
        }
        
        public void Refresh(Inventory.Slot e)
        {
            _equipment = e;
            var slot = Instantiate(equipmentDraggingPrefab, transform);
            slot.canvas = _canvas;
            slot.equipment = e;
            slot.pointToDrag = _pointToDrag;

        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent(out EquipmentDragging v))
            {
                // v.gameObject.transform.parent = transform;
                var item = v.equipment.item;
                v.DidRemoveItem();
                Destroy(v.gameObject);

                var slot = new Inventory.Slot(index, item);
                
                InventoryManager.main.AddSlot(slot);
                
                Refresh(slot);
            }
        }
    }
}