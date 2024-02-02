using UnityEngine;
using UnityEngine.Events;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "MessageChannel/CardEventMessageChannelSO", fileName = "CardEventMessageChannelSO")]
    public class CardEventMessageChannelSO: ScriptableObject
    {
        public UnityAction<CardPlacementCanvasMono> didSelected;


        public UnityAction<CardPlacementCanvasMono> outSelected;
        
        
        public UnityAction<CardDraggingMono> onDrag;
        public UnityAction<CardDraggingMono> onEndDrag;
        
        public void DidSelected(CardPlacementCanvasMono cardPlacementMono)
        {
            didSelected?.Invoke(cardPlacementMono);
        }

        
        public void OutSelected(CardPlacementCanvasMono cardPlacementMono)
        {
            outSelected?.Invoke(cardPlacementMono);
        }
        
        public void OnDrag(CardDraggingMono cardPlacementMono)
        {
            onDrag?.Invoke(cardPlacementMono);
        }
        public void OnEndDrag(CardDraggingMono cardPlacementMono)
        {
            onEndDrag?.Invoke(cardPlacementMono);
        }
    }
}