using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{


    // public static class SkillFounder
    // {
    //
    //     public static IEnumerable<ISkill> Found(string[] by)
    //     {
    //
    //         if (ReferenceEquals(by, null)) return  new ISkill[] {};
    //         
    //         if (by.IsNullOrEmpty()) return new ISkill[] {};
    //
    //         return by.Select(v =>
    //         {
    //             var array = v.Split("-");
    //
    //             return array.First() switch
    //             {
    //                 "dr" => new DamageReduceSkill(array[1]),
    //                 _ => new Skill() as ISkill
    //             };
    //         });
    //
    //
    //
    //     }
    //     
    // }
    
    // public interface ISkill
    // {
    //
    //     // public string skillCode { get; }
    //     
    //     public Attack SkillBy(Attack attack);
    //     
    //     
    //     public string skillName { get; }
    //     
    //     public string skillDescription { get; }
    //
    // }

    // public struct Skill: ISkill
    // {
    //     public string skillCode => "";
    //
    //     public Attack SkillBy(Attack attack) => attack;
    // }
    //
    // [Serializable]
    // public struct Xx : ISkill
    // {
    //     [SerializeField]
    //     public string skillCode { get; set; }
    //
    //     public string ll;
    //
    //     public Attack SkillBy(Attack attack)
    //     {
    //         return new Attack();
    //     }
    // }

    
}