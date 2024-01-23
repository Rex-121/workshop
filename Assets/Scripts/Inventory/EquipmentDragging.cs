using Sirenix.OdinInspector;
using Tyrant.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentDragging: MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        
        [ShowInInspector]
        private Inventory.Slot _equipment;

        public Image image;

        private Canvas _canvas;

        private Transform _pointToDrag;

        public CanvasGroup canvasGroup;

        
        public ItemInspectorMessageChannel messageChannel;

        // private GameObject _inspector;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _canvas = UIManager.main.canvas;
            _pointToDrag = UIManager.main.dragPointForItem;
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
            transform.SetParent(_pointToDrag);
            
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

            canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_temporaryPinned)
            {
                transform.SetParent(previousParent);    
            }
            // transform.SetParent(previousParent);    
            _rectTransform.anchoredPosition = Vector2.zero;
            canvasGroup.blocksRaycasts = true;
        }

        [ShowInInspector]
        private bool _temporaryPinned = false;
        public void TemporaryPin()
        {
            _temporaryPinned = true;
        }
        
        public void UnsetTemporaryPin()
        {
            _temporaryPinned = false;
            
            if (!_temporaryPinned)
            {
                transform.SetParent(previousParent);    
            }
            // transform.SetParent(previousParent);    
            _rectTransform.anchoredPosition = Vector2.zero;
            canvasGroup.blocksRaycasts = true;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // messageChannel.ItemInspector(_equipment.item);
            UIManager.main.InspectorItem(_equipment.item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // messageChannel.ItemInspector(null);
            UIManager.main.InspectorItem(null);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            messageChannel.ItemInspector(_equipment.item);
            // UIManager.main.PinInspectorItem(_equipment.item);
        }
    }
}