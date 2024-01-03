using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    
    [System.Serializable]
    public struct RawMaterial
    {
        [SerializeField]
        public string itemName { get; }

        public string id;
        
        [SerializeField]
        public Sprite sprite { get; set; }
        
        [SerializeField]
        public string code { get; set; }

        
        public MaterialFeature[] features;
        
        public RawMaterial(string name, Sprite sprite, string code, IEnumerable<MaterialFeature> features, Guid id)
        {
            this.id = id.ToString(); 
            itemName = name;
            this.sprite = sprite;
            this.code = code;
            this.features = features.ToArray();
        }

        public IMaterial toMaterial => new Material(this);
    }
    
    [System.Serializable]
    public struct Material: IMaterial
    {
        [ShowInInspector] public string itemName => rawMaterial.itemName + $"<sprite={quality.tier.ToInt()}>";
        
        [ShowInInspector] public Sprite sprite => rawMaterial.sprite;

        [ShowInInspector, ReadOnly]
        public string code => rawMaterial.code;

        [SerializeField]
        public string id;

        [ShowInInspector]
        public RawMaterial rawMaterial;

        public MaterialFeature[] features => rawMaterial.features;
        
        [SerializeField]
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
            id = rawMaterial.id;
        }
    }
}