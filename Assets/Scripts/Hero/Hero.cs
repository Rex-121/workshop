using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class Hero: IAmHero
    {
        public Attribute attribute { get; set; }
        
        [LabelText("生命值")]
        public Health health { get; set; }
        public string heroName { get; set; }

        public AttributeTypes mainAttribute;
        public IAmHero heroic => this;

        public HeroActionQueue actionQueue { get; private set; }


        [ShowInInspector, LabelText("伤害")]
        public AttackPower attackPower;
        
        public bool stillAlive => !health.isEmpty;

        public ISkill[] skills;

        public IWeapon weapon = new Sword(new Attribute(5, 5, 5), null);
        
        Hero(Attribute a, HeroHealthStrategy healthStrategy, AttributeTypes mainAttribute, string jobName, IEnumerable<ISkill> skills)
        {
            attribute = a;
            health = new Health(attribute, healthStrategy);
            
            this.mainAttribute = mainAttribute;
            heroName = $"[{jobName}]{NVJOBNameGen.Uppercase(NVJOBNameGen.GiveAName(7))}";

            actionQueue = new HeroActionQueue(this);
            
            attackPower = weapon.power + attributePower;

            this.skills = skills.ToArray();
        }

        public static Hero FromSO(JobSO jobSO)
        {
            return new Hero(jobSO.attribute, jobSO.healthStrategy, jobSO.mainAttribute, jobSO.jobName, jobSO.skills ?? new ISkill[] {});
        }
        private int attributePower => mainAttribute switch
        {
            AttributeTypes.Strength => Math.Max(0, (attribute.strength - 10) / 2) + 1,
            AttributeTypes.Dexterity => Math.Max(0, (attribute.dexterity - 10) / 2) + 1,
            AttributeTypes.Intelligence => Math.Max(0, (attribute.intelligence - 10) / 2) + 1,
            _ => 0
        };

        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = attackPower.power;
            battleVersus.TakeDamage(attack);
            return attack;
        }

        public void TakeDamage(Attack attack)
        {
            skills.ForEach(v =>
            {
                attack = v.SkillBy(attack);
            });
            
            var damage = attack.power;
            Debug.Log($"受到了 {damage} 点攻击");
            health.TakeDamage(attack);
        }
    }


    public interface IAmHero: IBattleVersus
    {
        public Attribute attribute { get; }
        
        public string heroName { get; }
        
        public IAmHero heroic { get; }
        
        public bool stillAlive { get; }
    }
}