using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "材料/特性", fileName = "材料特性")]
    public class MaterialFeatureSO: SerializedScriptableObject
    {

        [HideLabel, HorizontalGroup("Basic")]
        public string featureName;

        [HideLabel]
        [HorizontalGroup("Basic"), SuffixLabel("受影响的品质           ", overlay:true)]
        public Quality.Tier effectiveTiers;

        [LabelText("是否编辑内容")] public bool editUsage;
        
        
        [HideLabel, ShowInInspector, ShowIf("@editUsage"), TextArea] public string editUsageLabel;

        [HideLabel, ShowInInspector, PropertyOrder(100)]
        public string featureUsage
        {
            get
            {
                if (editUsage) return editUsageLabel;
                if (pinDice == null) return buffConfig?.description ?? "";
                return pinDice?.description ?? "";
            }
        }
        
        
        [LabelText("放下工具时"), BoxGroup("SKILL"), PropertyOrder(101)]
        public IMaterialFeature pinDice;
        [LabelText("Buff生效时"), BoxGroup("SKILL"), PropertyOrder(102)]
        public IMaterialFeature buffConfig;




        /// <summary>
        /// 此品质材料是否受影响
        /// </summary>
        /// <param name="quality">品质</param>
        /// <returns>是否受影响</returns>
        public bool IsEffective(Quality quality)
        {
            return effectiveTiers.HasFlag(quality.tier);
        }
    }
}