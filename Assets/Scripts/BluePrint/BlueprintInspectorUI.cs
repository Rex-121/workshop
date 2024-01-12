using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class BlueprintInspectorUI: MonoBehaviour, IPointerClickHandler
    {

        public BluePrint bluePrint;


        public Image image;


        public UnityAction<BluePrint> selection;

        public void NewBlueprint(BluePrint bp)
        {
            bluePrint = bp;
            image.sprite = bp.icon;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            selection?.Invoke(bluePrint);
        }
    }
}