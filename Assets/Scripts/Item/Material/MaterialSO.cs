using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "材料/原料", fileName = "原料")]
    public class MaterialSO : ScriptableObject
    {
        
        public string name;
        
        public Sprite icon;

        public string code;

        [LabelText("特性")]
        public MaterialFeatureSO[] featureSOs;
        
        public RawMaterial toRawMaterial => new (name, icon, code, featureSOs.Select(MaterialFeatureMakes.From));

    }
}
