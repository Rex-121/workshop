using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentBriefDisplay: MonoBehaviour
    {

        public Image icon;

        public void Equipment(IEquipment equipment)
        {
            icon.sprite = equipment.sprite;
        }

    }
}