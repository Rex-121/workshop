using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tyrant
{
    public readonly struct AttackPower
    {

        [ShowInInspector, ReadOnly]
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
    }


    public readonly struct Attack
    {
        private readonly int _power;

        public Color damageColor => isCritical ? Color.red : Color.white;

        public int damage
        {
            get
            {
                if (_criticalStrategy == null) return _power;
                return (int)Math.Ceiling(_criticalStrategy.rate * _power);
            }
        }

        private readonly ICriticalStrategy _criticalStrategy;


        // 是否暴击
        public bool isCritical => _criticalStrategy != null;

        public Attack(int power, ICriticalStrategy strategy = null)
        {
            this._power = power;
            _criticalStrategy = strategy;
        }

        public Attack Critical(ICriticalStrategy strategy)
        {
            return new Attack(_power, strategy);
        }
        
        public Attack Critical()
        {
            return new Attack(_power, new CriticalRate2());
        }
    }

    public interface ICriticalStrategy
    {
        public float rate { get; }
    }


    public struct CriticalRate2: ICriticalStrategy
    {
        public float rate => 2.0f;
    }
    
    
    
    
}