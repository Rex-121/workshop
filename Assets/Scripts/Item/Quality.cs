using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{

    [InlineEditor, HideReferenceObjectPicker, HideLabel]
    public interface IQuality
    {
        public Quality quality { get; }
    }

    [Serializable]
    public struct QualityGroup: IQuality
    {
        
        // ReSharper disable once UnusedMember.Local
        private string v => $"品级({quality.tier.debugDescription()})";
        
        [TitleGroup("最终品质", "$v")]
        [FoldoutGroup("最终品质/品质集")]
        [HideLabel]
        [HorizontalGroup("最终品质/品质集/品质")]
        [VerticalGroup("最终品质/品质集/品质/基本品质")]
        [BoxGroup("最终品质/品质集/品质/基本品质/基本品质")]
        [ShowInInspector]
        public IQuality make;
        
        [FoldoutGroup("最终品质/品质集")]
        [HorizontalGroup("最终品质/品质集/品质")]
        [ShowInInspector, LabelText("品质集")]
        public List<IQuality> adds;

        [VerticalGroup("最终品质/品质集/品质/基本品质"), ShowInInspector, SuffixLabel("算法", true), HideLabel]
        private readonly IQualityMakeStrategy<QualityGroup> _strategy;
        
        public QualityGroup(IQuality a, IEnumerable<IQuality> b, IQualityMakeStrategy<QualityGroup> s)
        {
            make = a.quality;
            _strategy = s;
            adds = b.Select(v => v.quality as IQuality).ToList();
        }

        // 最终品质
        public Quality quality => _strategy.QualityBy(this).quality;
    }
    
    /// <summary>
    /// 标准的 制作+品质 进度计算
    /// </summary>
    public readonly struct NormalMakeWithQualityStrategy : IQualityMakeStrategy<QualityGroup>
    {
        public IQuality QualityBy(QualityGroup item)
        {
            if (item.make.quality.tier == Quality.Tier.Defectives) return item.make.quality;
            var allQuality = item.adds.Sum(v => v.quality.tier.ToInt()) - Quality.Tier.Fine.ToInt();
            var all = allQuality + item.make.quality.tier.ToInt();
            return Quality.On(all.TierFromInt());
        }
    }
    
    public static class TierExt
    {
        public static Quality.Tier TierFromInt(this int value)
        {
            return value switch
            {
                0 => Quality.Tier.Defectives,
                1 => Quality.Tier.Fine,
                2 => Quality.Tier.Superior,
                3 => Quality.Tier.Masterpiece,
                >= 4 => Quality.Tier.Legendary,
                _ => Quality.Tier.Defectives
            };
        }
        
        public static int ToInt(this Quality.Tier o)
        {
            return o switch
            {
                Quality.Tier.Defectives => 0,
                Quality.Tier.Fine => 1,
                Quality.Tier.Superior => 2,
                Quality.Tier.Masterpiece => 3,
                Quality.Tier.Legendary => 4,
                _ => 0
            };
        }

        public static string debugDescription(this Quality.Tier o)
        {
            return o switch
            {
                Quality.Tier.Defectives => $"{o}",
                Quality.Tier.Fine => $"{o}*",
                Quality.Tier.Superior => $"{o}**",
                Quality.Tier.Masterpiece => $"{o}***",
                Quality.Tier.Legendary => $"{o}****",
                _ => $"{o}"
            };
        }
        
        public static Quality ToQuality(this Quality.Tier o)
        {
            return Quality.On(o);
        }
    }
    
    [Serializable]
    public struct Quality: IQuality
    {
        [SerializeField] public Tier tier;
        
        private Quality(Tier t)
        {
            tier = t;
        }

        public static Quality On(Tier t) => new (t);

        public static IQuality operator +(Quality a, IQuality b)
        {
            var value = a.tier.ToInt() + b.quality.tier.ToInt();
            return On(value.TierFromInt());
        }

        public override string ToString() => $"品级{tier}";

        [HideLabel, SuffixLabel("品级  ", true), Serializable]
        public enum Tier
        {
            Defectives,
            Fine, 
            Superior, 
            Masterpiece, 
            Legendary,
        }

        public Quality quality => this;

        public static Quality Random()
        {
            return new Quality(UnityEngine.Random.Range(0, 5).TierFromInt());
        }
    }

    
}
