using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class ItemRequireForInventorySlot: MonoBehaviour
    {
        // public RectTransform rect;
        
        [ShowInInspector]
        public RawMaterial item;
        
        public TextMeshProUGUI itemNameLabel;
        public Image icon;
        
        
        public void AddItem(RawMaterial? iItem)
        {
            if (iItem == null) return;
            
            item = iItem!.Value;
            
            Refresh();
        }
        private void Refresh()
        {
            itemNameLabel.text = item.itemName;
            icon.sprite = item.sprite;
        }

        public void Clear()
        {
            Destroy(gameObject);
        }
        
    }
}