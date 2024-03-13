using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace Tyrant
{
    public class CardDraggingMono: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {

        

        public CardEventMessageChannelSO messageChannelSO;

        public bool isDragging = false;

        public Canvas canvas;

        [LabelText("骰子Prefab")]
        public GameObject dice;
        [LabelText("拖拽的骰子")]
        public GameObject draggingDice;

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            WorkBenchManager.main.ToolIsSelected(null);
            messageChannelSO.OnEndDrag(this);
            isDragging = false;

            draggingDice.transform
                .DOMove(dice.transform.position, 0.2f)
                .OnComplete(() =>
                {
                    Destroy(draggingDice);
                    dice.SetActive(true);
                });

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            WorkBenchManager.main.ToolIsSelected(GetComponent<CardInfoMono>().tool);
            messageChannelSO.OnBeginDrag(this);
            draggingDice = Instantiate(dice, dice.transform.position, Quaternion.identity, canvas.transform);
            dice.SetActive(false);
            draggingDice.GetComponent<CanvasGroup>().blocksRaycasts = false;
            isDragging = true;
        }

        private void Update()
        {
            if (isDragging && !ReferenceEquals(draggingDice, null))
            {
                draggingDice.transform.position = Input.mousePosition;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}