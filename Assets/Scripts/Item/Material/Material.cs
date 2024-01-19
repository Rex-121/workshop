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
        
        public MaterialType type => _materialSO.type;

        private MaterialSO _materialSO => ItemGenesis.main.FindMaterialSOById(id);

        public MaterialFeatureSO[] features => _materialSO.featureSOs;
        
        public RawMaterial(MaterialSO so)
        {
            id = so.id.ToString(); 
        }

        public IMaterial toMaterial => new Material(this);
    }
    
    [System.Serializable]
    public struct Material: IMaterial
    {
        [ShowInInspector] public string itemName => rawMaterial.itemName;
        
        [ShowInInspector] public Sprite sprite => rawMaterial.sprite;

        public string id => rawMaterial.id;

        [ShowInInspector]
        public RawMaterial rawMaterial;

        [ShowInInspector]
        public MaterialFeatureSO[] features => rawMaterial.features;
        
        public MaterialType type => rawMaterial.type;
        
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