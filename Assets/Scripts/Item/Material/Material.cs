using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    
    public struct RawMaterial
    {
        [ShowInInspector]
        public string itemName { get; }
        
        
        [ShowInInspector]
        public Sprite sprite { get; set; }
        
        public string code { get; set; }
        
        public RawMaterial(string name, Sprite sprite, string code)
        {
            itemName = name;
            this.sprite = sprite;
            this.code = code;
        }

        public IMaterial toMaterial => new Material(this);
    }
    
    public struct Material: IMaterial
    {
        [ShowInInspector] public string itemName => rawMaterial.itemName;


        [ShowInInspector] public Sprite sprite => rawMaterial.sprite;

        [ShowInInspector, ReadOnly]
        public string code => rawMaterial.code;

        [ShowInInspector]
        public RawMaterial rawMaterial;

        public Quality quality => Quality.On(Quality.Tier.Fine);

        public Material(string name, Sprite sprite, string code)
        {
            rawMaterial = new RawMaterial(name, sprite, code);
        }
        
        public Material(RawMaterial rawMaterial)
        {
            this.rawMaterial = rawMaterial;
        }
    }
}