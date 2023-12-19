using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class InventorySlot: MonoBehaviour, IDropHandler
    {

        public ItemPreviewForInventorySlot previewPrefab;

        private ItemPreviewForInventorySlot _previewItem;

        [ShowInInspector]
        public IInventorySlotDrag handler;


        public ItemPreviewForInventorySlot.IInventoryItemDragging ItemDraggingHandle;
        
        public void AddItem(IItem item)
        {
            if (ReferenceEquals(item, null))
            {
                Remove();
            }
            else
            {
                Refresh(item);
            }
        }

        public void Clear() => AddItem(null);

        private void Remove()
        {
            Destroy(_previewItem);
            _previewItem = null;
        }
        
        private void Refresh(IItem item)
        {
            _previewItem = Instantiate(previewPrefab, transform).GetComponent<ItemPreviewForInventorySlot>();
            _previewItem.handler = ItemDraggingHandle;
            _previewItem.AddItem(item);
        }

        // 是否有空位
        public bool isOccupied => _previewItem != null;

        public void OnDrop(PointerEventData eventData)
        {
            if (ReferenceEquals(handler, null)) return;
            
            var obj = eventData.pointerDrag;

            var has = obj.TryGetComponent(out ItemPreviewForInventorySlot itemPreview);
            
            if (!has) return;
            
            handler?.OnDrop(itemPreview);
            
        }
        
        
        public interface IInventorySlotDrag
        {
            public void OnDrop(ItemPreviewForInventorySlot item);
        }
    }
}