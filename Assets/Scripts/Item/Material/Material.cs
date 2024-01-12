using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tyrant.Items;
using UnityEngine;

namespace Tyrant
{
    
    [Serializable]
    public struct RawMaterial
    {
        public string itemName => _materialSO.materialName;

        public string id;

        public Sprite sprite => _materialSO.icon;

        public string code => _materialSO.code;

        public MaterialType type => _materialSO.type;

        private MaterialSO _materialSO => ItemGenesis.main.FindMaterialSOById(id);
        
        public MaterialFeature[] features;
        
        public RawMaterial(MaterialSO so, IEnumerable<MaterialFeature> features)
        {
            this.id = so.id.ToString(); 
            // itemName = name;
            // this.sprite = sprite;
            // this.code = code;
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

        public string id => rawMaterial.id;

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

        public override string ToString()
        {
            return $"{base.ToString()}-{rawMaterial.itemName}-({quality})";
        }

        public Material(RawMaterial rawMaterial)
        {
            this.rawMaterial = rawMaterial;
            quality = Quality.Random();
            // id = rawMaterial.id;
        }
    }
}