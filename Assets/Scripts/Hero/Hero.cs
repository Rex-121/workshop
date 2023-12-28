using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tyrant
{
    public class Hero: IAmHero
    {
        public Attribute attribute { get; set; }
        
        [LabelText("生命值")]
        public Health health { get; set; }
        public string heroName { get; set; }

        // public AttributeTypes mainAttribute;
        public IAmHero heroic => this;

        public HeroActionQueue actionQueue { get; private set; }


        [ShowInInspector, LabelText("伤害")]
        public AttackPower attackPower;
        
        public bool stillAlive => !health.isEmpty;
        
        public BuffHandler buffHandler = new BuffHandler();

        [ShowInInspector]
        public IEquipment weapon;// = new Sword(new Attribute(5, 5, 5), null);

        [ShowInInspector]
        public JobSO job;
        
        Hero(Attribute a, HeroHealthStrategy healthStrategy, JobSO jobSO)//IEnumerable<BuffInfo> skills)
        {
            attribute = a + jobSO.weaponSO.attribute;
            
            health = new Health(attribute, healthStrategy);

            job = Object.Instantiate(jobSO);
            heroName = $"[{jobSO.jobName}]{NVJOBNameGen.Uppercase(NVJOBNameGen.GiveAName(7))}";

            actionQueue = new HeroActionQueue(this);
            
            weapon = jobSO.weaponSO.ToEquipment();


            attackPower = weapon.power + jobSO.AttributePower(attribute);

            (job.skills ?? new BuffDataSO[] {}).Select(v => v.ToBuff())
                .ForEach(v =>
            {
                buffHandler.AddBuff(v);
            });


        }

        public static Hero FromSO(CharacterSO characterSO, JobSO jobSO)
        {
            return new Hero(characterSO.attribute, characterSO.healthStrategy, jobSO);
        }
        // private int attributePower => mainAttribute switch
        // {
        //     AttributeTypes.Strength => Math.Max(0, (attribute.strength - 10) / 2) + 1,
        //     AttributeTypes.Dexterity => Math.Max(0, (attribute.dexterity - 10) / 2) + 1,
        //     AttributeTypes.Intelligence => Math.Max(0, (attribute.intelligence - 10) / 2) + 1,
        //     _ => 0
        // };

        public void BattleDidEnd()
        {
            actionQueue.Reset();
            buffHandler.buffs.ForEach(v => v.BattleDidEnd());
        }

        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = buffHandler.WillHit(attackPower.power);
            
            battleVersus.TakeDamage(attack);
            return attack;
        }

        public void TakeDamage(Attack attack)
        {
            attack = buffHandler.WillTakeDamage(attack);
            var damage = attack.damage;
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