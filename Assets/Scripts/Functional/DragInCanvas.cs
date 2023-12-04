using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DragInCanvas : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private RectTransform _transform;

        private CanvasGroup _canvasGroup;

        private Vector2 _originPosition;

        private bool _isLocked = false;

        public Action endDrag;
        
        public Action startDrag;
        public void Lock()
        {
            _isLocked = true;
        }
        
        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _originPosition = _transform.anchoredPosition;
        }

        public Canvas canvas;
        public void OnDrag(PointerEventData eventData)
        {
            startDrag?.Invoke();
            _transform.anchoredPosition += eventData.delta / 3.375f;// / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            
            // _transform.anchoredPosition = _originPosition;

            
            endDrag?.Invoke();

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
