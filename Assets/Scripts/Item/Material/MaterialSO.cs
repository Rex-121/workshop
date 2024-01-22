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
        
        [ReadOnly, HorizontalGroup("ID")]
        public Guid id;
         
        [HorizontalGroup("Info", width: 60), PreviewField(60), HideLabel]
        public Sprite icon;

        [HorizontalGroup("Info"), VerticalGroup("Info/Basic"), HideLabel]
        public string materialName;
       
        [SerializeField, HorizontalGroup("Info"), VerticalGroup("Info/Basic"), HideLabel]
        public MaterialType type;

        [LabelText("特性")]
        public MaterialFeatureSO[] featureSOs;
        
        public RawMaterial toRawMaterial => new (this);

        public IMaterial ToMaterial(Quality quality) => new Material(new (this), quality) ;
        
        [Button, HorizontalGroup("ID")]
        public void AssignGuid()
        {
            id = Guid.NewGuid();
        } 
    }
}
