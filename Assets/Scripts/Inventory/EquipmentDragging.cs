using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentDragging: MonoBehaviour, IDragHandler, IEndDragHandler
    {
        
        private IEquipment _equipment;

        public Image image;

        public Canvas canvas;

        public Transform pointToDrag;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        private RectTransform _rectTransform;
        
        public IEquipment equipment
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
            
            image.sprite = equipment.sprite;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.SetParent(pointToDrag);
            _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(previousParent);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}