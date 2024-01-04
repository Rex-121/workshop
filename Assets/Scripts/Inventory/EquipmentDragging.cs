using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentDragging: MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        
        [ShowInInspector]
        private Inventory.Slot _equipment;

        public Image image;

        public Canvas canvas;

        public Transform pointToDrag;

        public CanvasGroup canvasGroup;



        // private GameObject _inspector;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        private RectTransform _rectTransform;
        
        public Inventory.Slot equipment
        {
            get => _equipment;
            set
            {
                _equipment = value;
                Refresh();
            }
        }

        public Transform previousParent;

        private void Refresh()
        {
            previousParent = transform.parent;
            
            image.sprite = equipment.item.sprite;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.SetParent(pointToDrag);
            
            // if (canvas == null) return;
            
            _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(previousParent);
            _rectTransform.anchoredPosition = Vector2.zero;
            canvasGroup.blocksRaycasts = true;
        }


        public void DidRemoveItem()
        {
            InventoryManager.main.Remove(equipment);
        }
        
                
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.main.InspectorItem(_equipment.item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.main.InspectorItem(null);
        }


    }
}