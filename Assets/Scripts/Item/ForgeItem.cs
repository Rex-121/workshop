using UniRx;
using UnityEngine;

namespace Tyrant
{

    public class ForgeItem
    {
        public StrikePower make = new StrikePower(Strike.Shape, 0);
        public StrikePower quality = new StrikePower(Strike.Quality, 0);

        [HideInInspector]
        public ReactiveProperty<int> makeStream = new(0);
        [HideInInspector]
        public ReactiveProperty<int> qualityStream = new(0);

        public BluePrint bluePrint;

        public float makePercent => make.power * 1.0f / bluePrint.make;
        public float qualityPercent => quality.power * 1.0f / bluePrint.quality;
        


        public NormalQualityStrategy qualityStrategy = new NormalQualityStrategy();
        public NormalMakeStrategy normalMakeQualityStrategy = new NormalMakeStrategy();
        
        public ForgeItem(BluePrint bluePrint)
        {
            this.bluePrint = bluePrint;
        }

        public void NewStrike(StrikePower strikePower)
        {
            Debug.Log($"#打造# 增加{strikePower.debugDescription}");
            make += strikePower;
            quality += strikePower;
            makeStream.Value = make.power;
            qualityStream.Value = quality.power;
        }
        
        
        public IEquipment DoForge()
        {
            var makes = normalMakeQualityStrategy.QualityBy(this);
            var qualities = qualityStrategy.QualityBy(this);
            Debug.Log($"#打造# 物品打造等级{makes.quality.tier} - 品质等级{qualities.quality.tier}");
            return EquipmentGenesis.main.DoCraft(makes, qualities, bluePrint.equipmentSO);
        }
        
    }
    
    public interface IQualityMakeStrategy<in T> //: IQuality
    {
        public IQuality QualityBy(T item);
    }


    /// <summary>
    /// 标准的制作进度计算
    /// 完成`make`点数即为`Tier.Fine`
    /// 反之为`Tier.Defectives`
    /// </summary>
    public readonly struct NormalMakeStrategy : IQualityMakeStrategy<ForgeItem>
    {
        public IQuality QualityBy(ForgeItem item) => 
            Quality.On(item.makePercent >= 1 ? Quality.Tier.Fine : Quality.Tier.Defectives);
    }

    /// <summary>
    /// 标准的品质进度计算
    /// 初始即为`Tier.Fine`
    /// </summary>
    public readonly struct NormalQualityStrategy : IQualityMakeStrategy<ForgeItem>
    {
        private static Quality.Tier Cal(float t)
        {
            Debug.Log("#打造# 原始Quality为" + t);
            if (t < 1)
            {
                var win = DoNeedLiftTier(t);
                return win ? Quality.Tier.Superior : Quality.Tier.Fine;
            }
            var left = Mathf.FloorToInt(t);
            Debug.Log(left);
            var value = Mathf.CeilToInt(t);
            Debug.Log($"#打造# 计算Quality为 {value}-{value.TierFromInt()}");
            var plus = DoNeedLiftTier(t - left);
            value += (plus ? 1 : 0);
            Debug.Log($"#打造# 追加Quality为 {value}-{value.TierFromInt()}");
            return value.TierFromInt();
        }

        // 计算是否要追加一等品级
        private static bool DoNeedLiftTier(float value)
        {
            var r = Random.Range(0, 1.0f);
            var v = r <= value;
            Debug.Log($"#打造# 计算是否要追加一等品级 几率{value}--投骰{r}--是否追加:{v}");
            return v;
        }
        
        public IQuality QualityBy(ForgeItem item)
        {
            return Quality.On(Cal(item.qualityPercent));
        }
    }



    
}
