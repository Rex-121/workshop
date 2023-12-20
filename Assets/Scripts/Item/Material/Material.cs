using System.Collections.Generic;
using System.Linq;
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

        
        public MaterialFeature[] features;
        
        public RawMaterial(string name, Sprite sprite, string code, IEnumerable<MaterialFeature> features)
        {
            itemName = name;
            this.sprite = sprite;
            this.code = code;
            this.features = features.ToArray();
        }

        public IMaterial toMaterial => new Material(this);
    }
    
    public struct Material: IMaterial
    {
        [ShowInInspector] public string itemName => rawMaterial.itemName + $"<sprite={quality.tier.ToInt()}>";
        
        [ShowInInspector] public Sprite sprite => rawMaterial.sprite;

        [ShowInInspector, ReadOnly]
        public string code => rawMaterial.code;

        [ShowInInspector]
        public RawMaterial rawMaterial;

        public MaterialFeature[] features => rawMaterial.features;
        public Quality quality { get; set; }

        // public Material(string name, Sprite sprite, string code)
        // {
        //     rawMaterial = new RawMaterial(name, sprite, code, new MaterialFeature[] { });
        //     quality = Quality.Random();
        // }
        
        public Material(RawMaterial rawMaterial)
        {
            this.rawMaterial = rawMaterial;
            quality = Quality.Random();
        }
    }
}