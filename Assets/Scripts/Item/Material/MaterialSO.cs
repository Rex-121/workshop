using System;
using System.Linq;
using Sirenix.OdinInspector;
using Tyrant.Items;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "材料/原料", fileName = "原料")]
    public class MaterialSO : SerializedScriptableObject
    {
        
        [ReadOnly]
        public Guid id;
        
        public string materialName;
        
        public Sprite icon;

        public string code;

        public MaterialType type;

        [LabelText("特性")]
        public MaterialFeatureSO[] featureSOs;
        
        public RawMaterial toRawMaterial => new (this, featureSOs.Select(MaterialFeatureMakes.From));

        
        
        [Button]
        public void AssignGuid()
        {
            id = Guid.NewGuid();
        } 
    }
}
