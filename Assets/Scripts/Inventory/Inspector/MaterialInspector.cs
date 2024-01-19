using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant.UI
{
    
    /*
     * 材料Inspector 主要显示材料的信息
     */
    public class MaterialInspector: ItemInspector
    {
        
        public Image icon;
        
        
        public TextMeshProUGUI nameLabel, qualityLabel;

        public TextMeshProUGUI materialFeatureLabel;
        
        public IMaterial material
        {
            set
            {
                nameLabel.text = value.itemName;
                icon.sprite = value.sprite;
                qualityLabel.text = value.quality.ToString();
                qualityLabel.color = value.quality.tier.Color();

                materialFeatureLabel.text = value.features.DebugDescription();

            }
        }
        
       

        public override void NewItem(IItem item)
        {
            base.NewItem(item);

            if (item is IMaterial e)
            {
                material = e;
            }
        }
        
    }
}