using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    [HideReferenceObjectPicker]
    public class EquipmentBriefDisplay: MonoBehaviour
    {

        public Image icon;

        public void Equipment(IEquipment equipment)
        {
            icon.sprite = equipment?.sprite;
            icon.enabled = icon.sprite != null;
        }
    }
}