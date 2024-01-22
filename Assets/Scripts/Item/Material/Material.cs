using System;
using System.Collections.Generic;
using System.Linq;
using Item.Material;
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

        public IMaterial ToMaterial(Quality quality, IMaterialFeatureGenerate featureGenerate) 
            => new Material(this, quality, featureGenerate);

    }
    
    [System.Serializable]
    public struct Material: IMaterial
    {
        [ShowInInspector] public string itemName => rawMaterial.itemName;
        
        [ShowInInspector] public Sprite sprite => rawMaterial.sprite;

        public string id => rawMaterial.id;

        [ShowInInspector]
        public RawMaterial rawMaterial;

        [SerializeField]
        public string[] featureNames;

        [ShowInInspector]
        public MaterialFeatureSO[] features => featureNames
            .Select(ItemGenesis.main.FindMaterialFeatureSOByName)
            .ToArray();
        
        public MaterialType type => rawMaterial.type;
        
        [SerializeField]
        public Quality quality { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}-{rawMaterial.itemName}-({quality})";
        }

        public Material(RawMaterial rawMaterial)
        {
            this.rawMaterial = rawMaterial;
            quality = Quality.Random();
            featureNames = new AtLeastOneFeatureGenerate()
                .GetFeatures(rawMaterial.features, quality).Select(v => v.name).ToArray();

        }
        
        public Material(RawMaterial rawMaterial, Quality quality, IMaterialFeatureGenerate featureGenerate)
        {
            this.rawMaterial = rawMaterial;
            this.quality = quality;
            featureNames = featureGenerate.GetFeatures(rawMaterial.features, quality).Select(v => v.name).ToArray();
        }
    }
}