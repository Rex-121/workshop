using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tyrant
{
    [Serializable]
    public readonly struct AttackPower: ILiftByQuality<AttackPower>
    {

        private readonly RangeInt _range;

        [LabelText("伤害"), ShowInInspector]
        public string predictPower => $"{_range.start} - {_range.end}";

        public AttackPower(RangeInt r)
        {
            _range = r;
        }

        public AttackPower(int from, int to)
        {
            _range = new RangeInt(from, to - from);
        }


        public Attack power => new (Random.Range(_range.start, _range.end + 1));
        
        
        public static AttackPower operator +(AttackPower first, int value)
        {
            return new AttackPower(first._range.start + value, first._range.end + value);
        }

        // 通过品质提升攻强
        public AttackPower LiftByQuality(IQuality qualities)
        {
            var i = qualities.quality.tier.ToInt();
            return this + i;
        }
    }


    
}