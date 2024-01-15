using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
namespace Tyrant
{
    public class EquipmentInventorySlot: ItemsDroppableMonoBehavior
    {
        public EquipmentDragging equipmentDraggingPrefab;

        public int index;
        
        private EquipmentDragging _equipmentDragging;
        
        public Inventory.Type inventoryType;
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
            InventoryManager.main.InventoryBy(inventoryType)
                .SlotBy(index)
                .Subscribe(Refresh)
                .AddTo(this);
        }

        protected override void ItemDidDrop(EquipmentDragging item)
        {

            if (item.equipment.index == index) return;

            var slot = item.equipment;
            
            // 移除拖拽物品
            Destroy(item.gameObject);

            Inventory.Slot? old = null;

            if (_equipmentDragging != null)
            {
                // 如果格子有旧物品
                old = _equipmentDragging.equipment;
            }
            
            DestroyDragging();
            
            // 增加新的
            InventoryManager.main.SwapSlot(slot, old, index);
            
            UIManager.main.InspectorItem(null);
        }
    }
}