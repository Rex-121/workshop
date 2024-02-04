using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "MessageChannel/CardEventMessageChannelSO", fileName = "CardEventMessageChannelSO")]
    public class CardEventMessageChannelSO: ScriptableObject
    {
        public UnityAction<CardPlacementCanvasMono> didSelected;


        public UnityAction<CardPlacementCanvasMono> outSelected;
        
        
        public UnityAction<CardDraggingMono> onBeginDrag;
        public UnityAction<CardDraggingMono> onEndDrag;

        [NonSerialized]
        private CardDraggingMono _cardDraggingMono;
        // public bool isAnyCardIsSelected
        // {
        //     get
        //     {
        //         if (_cardDraggingMono == null) return false;
        //         return _cardDraggingMono.GetComponent<CardPlacementCanvasMono>().isLock;
        //     }
        // }
        
        public void DidSelected(CardPlacementCanvasMono cardPlacementMono)
        {
            didSelected?.Invoke(cardPlacementMono);
        }

        
        public void OutSelected(CardPlacementCanvasMono cardPlacementMono)
        {
            outSelected?.Invoke(cardPlacementMono);
        }
        
        public void OnBeginDrag(CardDraggingMono cardPlacementMono)
        {
            _cardDraggingMono = cardPlacementMono;
            onBeginDrag?.Invoke(cardPlacementMono);
        }
        public void OnEndDrag(CardDraggingMono cardPlacementMono)
        {
            _cardDraggingMono = cardPlacementMono;
            onEndDrag?.Invoke(cardPlacementMono);
        }
    }
}