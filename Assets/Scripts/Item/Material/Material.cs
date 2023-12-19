using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [InlineEditor]
    public struct Material: IMaterial
    {
        [ShowInInspector]
        public string itemName { get; }
        
        
        [ShowInInspector]
        public Sprite sprite { get; set; }

        public Quality quality => Quality.On(Quality.Tier.Fine);

        public Material(string name, Sprite sprite)
        {
            itemName = name;
            this.sprite = sprite;
        }
    }
}