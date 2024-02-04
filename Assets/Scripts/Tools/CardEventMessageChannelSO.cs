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
            onBeginDrag?.Invoke(cardPlacementMono);
        }
        public void OnEndDrag(CardDraggingMono cardPlacementMono)
        {
            onEndDrag?.Invoke(cardPlacementMono);
        }
    }
}