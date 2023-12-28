using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Tyrant
{
    public interface IBuffModule
    {
        public void Apply(BuffInfo buffInfo, Attack attack = new Attack(), Action<Attack> attackHandler = null);
    }
    
    
    
    // 伤害减免
    [Serializable, HideReferenceObjectPicker]
    public struct DamageReduceSkill: IBuffModule
    {
        // public string skillCode => "dr";
        [SerializeField, PropertyOrder(-1), HideLabel, SuffixLabel("技能名称", true), HorizontalGroup("Skill")]
        public string skillName { get; set; }
        
        [LabelText("减伤"), HorizontalGroup("Skill")]
        public int reduce;

        [ShowInInspector, HideLabel]
        public string skillDescription => $"所有伤害减少{reduce}点";
        
        public Attack SkillBy(Attack attack)
        {
            return new Attack( Math.Max(attack.damage - reduce, 0));
        }

        public void Apply(BuffInfo buffInfo, Attack attack = new Attack(), Action<Attack> attackHandler = null)
        {
            Debug.Log($"`{skillName}`减免{reduce} -- {attack.damage}");
            var a = new Attack( Math.Max(attack.damage - reduce, 0));
            attackHandler?.Invoke(a);
        }
    }


    public struct CriThroughTurnsBuffModule: IBuffModule
    {

        public int criticalPerTurn;
        
        public void Apply(BuffInfo buffInfo, Attack attack = new Attack(), Action<Attack> attackHandler = null)
        {

            if (buffInfo.currentStack >= criticalPerTurn)
            {
                attackHandler?.Invoke(attack.Critical());
                buffInfo.currentStack = 0;
            }
            else
            {
                buffInfo.currentStack += 1;
            }
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