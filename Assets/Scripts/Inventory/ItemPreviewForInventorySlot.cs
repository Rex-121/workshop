using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class ItemPreviewForInventorySlot: MonoBehaviour, IDragHandler, IEndDragHandler
    {

        public RectTransform rect;
        
        [ShowInInspector]
        public IItem item;
        
        public TextMeshProUGUI itemNameLabel;
        public Image icon;

        public IInventoryItemDragging handler;

        public CanvasGroup canvasGroup;

        private Transform _storeParent;

        
        private void Start()
        {
            _storeParent = transform.parent;
        }

        public void AddItem(IItem iItem)
        {
            item = iItem;
            
            Refresh();
        }
        private void Refresh()
        {
            itemNameLabel.text = item?.itemName ?? "";
            icon.sprite = item?.sprite;
            icon.gameObject.SetActive(!ReferenceEquals(item, null));
        }

        public void Clear()
        {
            handler?.OnWillDestroy(this);
            Destroy(gameObject);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (ReferenceEquals(handler, null))
            {
                eventData.pointerDrag = null;
                return;
            }
            
            handler?.OnItemIsDragging(this);

            itemNameLabel.enabled = false;

            if (handler?.anchor != null)
            {
                transform.SetParent(handler.anchor);
            }

            canvasGroup.blocksRaycasts = false;
            
            rect.anchoredPosition += eventData.delta / 3.0f;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            itemNameLabel.enabled = true;
            transform.SetParent(_storeParent);
            canvasGroup.blocksRaycasts = true;
            rect.anchoredPosition = Vector2.zero;
        }
        
        
        public struct DefaultDragging: IInventoryItemDragging
        {
            public Transform anchor { get; }
            
            public DefaultDragging(Transform an)
            {
                anchor = an;
            }

            public void OnWillDestroy(ItemPreviewForInventorySlot eventData) { }

            public void OnItemIsDragging(ItemPreviewForInventorySlot item) { }
        }
        
        public interface IInventoryItemDragging
        {
            public Transform anchor { get; }

            public void OnWillDestroy(ItemPreviewForInventorySlot item);
            
            public void OnItemIsDragging(ItemPreviewForInventorySlot item);
        }
    }
}