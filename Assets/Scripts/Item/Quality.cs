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
        
        public static string Description(this Quality.Tier o)
        {
            return o switch
            {
                Quality.Tier.Defectives => "残次品",
                Quality.Tier.Fine => "普通",
                Quality.Tier.Superior => "精良",
                Quality.Tier.Masterpiece => "大师",
                Quality.Tier.Legendary => "传奇",
                _ => ""
            };
        }
        
        public static Color Color(this Quality.Tier o)
        {
            return o switch
            {
                Quality.Tier.Defectives => new Color(124 / 255.0f,135 / 255.0f,138 /255.0f),
                Quality.Tier.Fine => new Color(199/255.0f,205/255.0f,242/255.0f),
                Quality.Tier.Superior => new Color(43/255.0f,217/255.0f,198/255.0f),
                Quality.Tier.Masterpiece => new Color(43/255.0f,125/255.0f,217/255.0f),
                Quality.Tier.Legendary => new Color(99/255.0f,75/255.0f,219/255.0f),
                _ => UnityEngine.Color.white
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
        
        public static Quality Fine => Quality.On(Quality.Tier.Fine);

        public static IQuality operator +(Quality a, IQuality b)
        {
            var value = a.tier.ToInt() + b.quality.tier.ToInt();
            return On(value.TierFromInt());
        }

        public override string ToString() => tier.Description();

        [HideLabel, SuffixLabel("品级  ", true), Serializable, Flags]
        public enum Tier
        {
            Defectives = 1,
            Fine = 2, 
            Superior = 4, 
            Masterpiece = 8, 
            Legendary = 16,
        }

        public Quality quality => this;

        public static Quality Random()
        {
            return new Quality(UnityEngine.Random.Range(0, 5).TierFromInt());
        }
    }

    
}
