
namespace Tyrant
{
    public interface IQualityMakeStrategy<in T> //: IQuality
    {
        public IQuality QualityBy(T item);
    }


    // /// <summary>
    // /// 标准的制作进度计算
    // /// 完成`make`点数即为`Tier.Fine`
    // /// 反之为`Tier.Defectives`
    // /// </summary>
    // public readonly struct NormalMakeQualityStrategy : IQualityMakeStrategy<ForgeProgressItem>
    // {
    //     public IQuality QualityBy(ForgeProgressItem item)
    //     {
    //         var f = item.make.power / item.PowerNeeded(Strike.Shape);
    //         return Quality.On(f >= 1 ? Quality.Tier.Fine : Quality.Tier.Defectives);
    //     }
    // }
    //
    // /// <summary>
    // /// 标准的品质进度计算
    // /// 初始即为`Tier.Fine`
    // /// </summary>
    // public readonly struct NormalQualityStrategy : IQualityMakeStrategy<ForgeProgressItem>
    // {
    //     private static Quality.Tier Cal(float t)
    //     {
    //         Debug.Log("原始Quality为" + t);
    //         if (t < 1)
    //         {
    //             var win = DoNeedLiftTier(t);
    //             return win ? Quality.Tier.Superior : Quality.Tier.Fine;
    //         }
    //         var left = Mathf.FloorToInt(t);
    //         Debug.Log(left);
    //         var value = Mathf.CeilToInt(t);
    //         Debug.Log($"计算Quality为 {value}-{value.TierFromInt()}");
    //         var plus = DoNeedLiftTier(t - left);
    //         value += (plus ? 1 : 0);
    //         Debug.Log($"追加Quality为 {value}-{value.TierFromInt()}");
    //         return value.TierFromInt();
    //     }
    //
    //     // 计算是否要追加一等品级
    //     private static bool DoNeedLiftTier(float value)
    //     {
    //         var r = Random.Range(0, 1.0f);
    //         var v = r <= value;
    //         Debug.Log($"计算是否要追加一等品级 几率{value}--投骰{r}--是否追加:{v}");
    //         return v;
    //     }
    //     
    //     public IQuality QualityBy(ForgeProgressItem item)
    //     {
    //         var hadPoints = item.quality.power;
    //         var needPoints = item.PowerNeeded(Strike.Quality);
    //         var f = hadPoints * 1.0f / needPoints;
    //         return Quality.On(Cal(f));
    //     }
    // }



    
}
