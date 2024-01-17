using TMPro;
using UnityEngine.UI;

namespace Tyrant.UI
{
    public class MaterialInspector: ItemInspector
    {
        
        public IMaterial material
        {
            set
            {
                nameLabel.text = value.itemName;
                icon.sprite = value.sprite;
                qualityLabel.text = value.quality.ToString();
            }
        }
        
        public Image icon;
        public TextMeshProUGUI nameLabel, qualityLabel;

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