using UnityEngine;
using UnityEngine.Events;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "MessageChannel/CardEventMessageChannelSO", fileName = "CardEventMessageChannelSO")]
    public class CardEventMessageChannelSO: ScriptableObject
    {
        public UnityAction<CardPlacementMono> didSelected;


        public UnityAction<CardPlacementMono> outSelected;
        
        public void DidSelected(CardPlacementMono cardPlacementMono)
        {
            didSelected?.Invoke(cardPlacementMono);
        }

        
        public void OutSelected(CardPlacementMono cardPlacementMono)
        {
            outSelected?.Invoke(cardPlacementMono);
        }
    }
}