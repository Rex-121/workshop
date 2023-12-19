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
            Destroy(gameObject);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (ReferenceEquals(handler, null))
            {
                eventData.pointerDrag = null;
                return;
            }

            if (handler.anchor != null)
            {
                transform.SetParent(handler.anchor);
            }

            canvasGroup.blocksRaycasts = false;
            
            rect.anchoredPosition += eventData.delta / 3.0f;
            // handler?.OnDrag(eventData);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
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
        }
        
        public interface IInventoryItemDragging
        {
            public Transform anchor { get; }

            // public void OnDrag(PointerEventData eventData);
            //
            // public void OnEndDrag(PointerEventData eventData);
        }
    }
}