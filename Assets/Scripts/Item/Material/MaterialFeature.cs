using UnityEngine;

namespace Tyrant
{


    public static class MaterialFeatureMakes
    {
        public static MaterialFeature From(MaterialFeatureSO so)
        {
            return new MaterialFeature(so.featureName, so.icon);
        }
    }
    
    
    public struct MaterialFeature
    {
        public string featureName;

        public Sprite icon;
        
        internal MaterialFeature(string featureName, Sprite icon)
        {
            this.featureName = featureName;
            this.icon = icon;
        }
    }
}