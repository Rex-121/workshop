using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "材料/特性", fileName = "材料特性")]
    public class MaterialFeatureSO: SerializedScriptableObject
    {
        public string featureName;

        public Sprite icon;

        [TextArea]
        public string featureUsage;
        
        
        public IMaterialFeature pinDice;

        public IMaterialFeature buffConfig;
    }
}