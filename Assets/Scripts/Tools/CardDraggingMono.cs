using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class CardDraggingMono: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {

        public GameObject dice;

        public CardEventMessageChannelSO messageChannelSO;

        public bool isDragging = false;

        public Canvas canvas;

        public GameObject d;

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            messageChannelSO.OnEndDrag(this);
            isDragging = false;

            d.transform
                .DOMove(dice.transform.position, 0.2f)
                .OnComplete(() =>
                {
                    Destroy(d);
                    dice.SetActive(true);
                });

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            messageChannelSO.OnBeginDrag(this);
            d = Instantiate(dice, dice.transform.position, Quaternion.identity, canvas.transform);
            dice.SetActive(false);
            isDragging = true;
        }

        private void Update()
        {
            if (isDragging && !ReferenceEquals(d, null))
            {
                d.transform.position = Input.mousePosition;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}