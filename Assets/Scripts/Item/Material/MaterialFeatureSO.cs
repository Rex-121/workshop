using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "材料/特性", fileName = "材料特性")]
    public class MaterialFeatureSO: ScriptableObject
    {
        public string featureName;

        public Sprite icon;

        [TextArea]
        public string featureUsage;
    }
}