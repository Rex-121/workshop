using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class InventorySlot: MonoBehaviour, IDropHandler, ItemPreviewForInventorySlot.IInventoryItemDragging
    {

        [LabelText("展示道具"), BoxGroup("Prefabs")]
        public ItemPreviewForInventorySlot previewPrefab;
        [LabelText("需求道具"),BoxGroup("Prefabs")]
        public ItemRequireForInventorySlot requirePrefab;
        [LabelText("特性展示"),BoxGroup("Prefabs")]
        public MaterialFeatureDisplayPanel featurePrefab;
        
        private RawMaterial? _rawMaterial;
            
        [ShowInInspector, LabelText("物品")]
        public ItemPreviewForInventorySlot previewItem { get; private set; }
        private ItemRequireForInventorySlot _requireItem;
        private MaterialFeatureDisplayPanel _featureItem;
            
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
            if (_rawMaterial == null || _requireItem != null) return;
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
            Destroy(previewItem.gameObject);
            previewItem = null;
            RemoveMaterialFeature();
        }

        private void RemoveMaterialFeature()
        {
            if (ReferenceEquals(_featureItem, null)) return;
            Destroy(_featureItem.gameObject);
            _featureItem = null;
        }
        
        private void Refresh(IItem item)
        {
            previewItem = Instantiate(previewPrefab, transform).GetComponent<ItemPreviewForInventorySlot>();
            previewItem.handler = this;
            previewItem.AddItem(item);

            if (item is Material material)
            {
                _featureItem = Instantiate(featurePrefab, transform).GetComponent<MaterialFeatureDisplayPanel>();
                _featureItem.materialFeature = material.features;
            }
            
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

        public Transform anchor => itemDraggingHandle.anchor;

        public void OnWillDestroy(ItemPreviewForInventorySlot item)
        {
            AddItem(null);
            RemoveMaterialFeature();
        }

        public void OnItemIsDragging(ItemPreviewForInventorySlot item)
        {
            
        }

        public void OnItemEndDragging(ItemPreviewForInventorySlot item)
        {
            _featureItem.gameObject.SetActive(true);
        }

        public void OnItemBeginDrag(ItemPreviewForInventorySlot item)
        {
            _featureItem.gameObject.SetActive(false);
            DisplayRequireIfNeeded();
        }
    }
}