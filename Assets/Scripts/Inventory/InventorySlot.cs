using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class InventorySlot: MonoBehaviour, IDropHandler
    {

        [LabelText("展示道具"), BoxGroup("Prefabs")]
        public ItemPreviewForInventorySlot previewPrefab;
        [LabelText("需求道具"),BoxGroup("Prefabs")]
        public ItemRequireForInventorySlot requirePrefab;
        private RawMaterial? _rawMaterial;
            
        [ShowInInspector, LabelText("物品")]
        public ItemPreviewForInventorySlot previewItem { get; private set; }
        private ItemRequireForInventorySlot _requireItem;
            
        [ShowInInspector, BoxGroup("Delegate")]
        public IInventorySlotDrag handler;

        [ShowInInspector, BoxGroup("Delegate")]
        public ItemPreviewForInventorySlot.IInventoryItemDragging itemDraggingHandle;


        public bool AddItemIfPossible(IItem item)
        {
            if (isOccupied) return false;
            AddItem(item);
            return true;
        }
        
        private void AddItem(IItem item)
        {
            if (ReferenceEquals(item, null))
            {
                Remove();
                DisplayRequireIfNeeded();
            }
            else
            {
                Refresh(item);
            }
        }
        
        public void AddRequire(RawMaterial rawMaterial)
        {
            _rawMaterial = rawMaterial;
            DisplayRequireIfNeeded();
        }

        private void DisplayRequireIfNeeded()
        {
            if (_rawMaterial == null) return;
            _requireItem = Instantiate(requirePrefab, transform).GetComponent<ItemRequireForInventorySlot>();
            _requireItem.AddItem(_rawMaterial);
        }

        private void RemoveRequireIfNeeded()
        {
            if (_requireItem == null) return;
            Destroy(_requireItem.gameObject);
        }
        
        private void Remove()
        {
            Destroy(previewItem);
            previewItem = null;
        }
        
        private void Refresh(IItem item)
        {
            previewItem = Instantiate(previewPrefab, transform).GetComponent<ItemPreviewForInventorySlot>();
            previewItem.handler = itemDraggingHandle;
            previewItem.AddItem(item);
            RemoveRequireIfNeeded();
        }

        // 是否有空位
        public bool isOccupied => previewItem != null;

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