using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Sirenix.Utilities;
using Tyrant;
using UnityEngine;

namespace Item.Material
{
    public interface IMaterialFeatureGenerate
    {
        public IEnumerable<MaterialFeatureSO> GetFeatures(MaterialFeatureSO[] featureSos, Quality quality);
    }




    // 普通材料生成器，随机生成
    // 至少一个
    public struct AtLeastOneFeatureGenerate : IMaterialFeatureGenerate
    {

        public IEnumerable<MaterialFeatureSO> GetFeatures(MaterialFeatureSO[] featureSos, Quality quality)
        {
            var range = new RangeInt(1, 2);
            if (featureSos.IsNullOrEmpty()) return new MaterialFeatureSO[] { };
            
            var random = range.RandomInRange();
            var count = Mathf.Min(random, featureSos.Length);
            
            var stack = new Stack(featureSos.Shuffled().ToArray());

            var array = new List<MaterialFeatureSO>();

            for (var i = 0; i < count; i++)
            {
                while (stack.Count != 0)
                {
                    var feature = stack.Pop() as MaterialFeatureSO;
                    if (feature == null || !feature.IsEffective(quality)) continue;
                    array.Add(feature);
                    break;
                }
            }

            return array;
        }
        
    }
    
}