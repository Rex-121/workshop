using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
namespace Tyrant
{
    public class EquipmentInventorySlot: MonoBehaviour, IDropHandler
    {
        public EquipmentDragging equipmentDraggingPrefab;

        public int index;
        
        private EquipmentDragging _equipmentDragging;
        private void Refresh(Inventory.Slot e)
        {

            DestroyDragging();
            
            if (e.item == null) return;
            
            _equipmentDragging = Instantiate(equipmentDraggingPrefab, transform);
            _equipmentDragging.equipment = e;
        }

        private void DestroyDragging()
        {
            if (_equipmentDragging == null) return;
            
            Destroy(_equipmentDragging.gameObject);
            _equipmentDragging = null;
        }

        private void Start()
        {
            InventoryManager.main.equipments
                .SlotBy(index)
                .Subscribe(Refresh)
                .AddTo(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out EquipmentDragging v)) return;

            if (v.equipment.index == index) return;

            var slot = v.equipment;
            
            // v.DidRemoveItem();
            Destroy(v.gameObject);

            Inventory.Slot? old = null;

            if (_equipmentDragging != null)
            {
                old = _equipmentDragging.equipment;
            }
            
            DestroyDragging();
            
            // 增加新的
            InventoryManager.main.SwapSlot(slot, old, index);
            
            UIManager.main.InspectorItem(null);
        }
    }
}