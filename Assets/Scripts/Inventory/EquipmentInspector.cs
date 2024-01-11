using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class EquipmentInspector: MonoBehaviour
    {

        public IEquipment equipment
        {
            set
            {
                nameLabel.text = value.itemName;
                icon.sprite = value.sprite;
                powerLabel.text = value.power.predictPower;
                qualityLabel.text = value.quality.ToString();
                var att = value.attribute;
                sLabel.text = att.strength.ToString();
                dLabel.text = att.dexterity.ToString();
                iLabel.text = att.intelligence.ToString();
            }
        }

        public Image icon;
        public TextMeshProUGUI nameLabel, powerLabel, qualityLabel;
        public TextMeshProUGUI sLabel, dLabel, iLabel;

    }
}