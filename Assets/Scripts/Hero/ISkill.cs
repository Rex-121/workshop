using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

namespace Tyrant
{


    public static class SkillFounder
    {

        public static IEnumerable<ISkill> Found(string[] by)
        {

            if (ReferenceEquals(by, null)) return  new ISkill[] {};
            
            if (by.IsNullOrEmpty()) return new ISkill[] {};

            return by.Select(v =>
            {
                var array = v.Split("-");

                return array.First() switch
                {
                    "dr" => new DamageReduceSkill(array[1]),
                    _ => new Skill() as ISkill
                };
            });



        }
        
    }
    
    public interface ISkill
    {

        public string skillCode { get; }
        
        public Attack SkillBy(Attack attack);

    }

    public struct Skill: ISkill
    {
        public string skillCode => "";

        public Attack SkillBy(Attack attack) => attack;
    }


    // 伤害减免
    public readonly struct DamageReduceSkill : ISkill
    {
        public string skillCode => "dr";

        private readonly int _reduce;
        
        public DamageReduceSkill(int r)
        {
            _reduce = r;
        }
        
        public DamageReduceSkill(string r)
        {
            _reduce = int.Parse(r);
        }


        public Attack SkillBy(Attack attack)
        {
            return new Attack( Math.Max(attack.power - _reduce, 0));
        }
    }
    
    
    
}