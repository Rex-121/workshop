using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tyrant
{
    [HideReferenceObjectPicker, InlineProperty, HideLabel, BoxGroup("HERO", centerLabel: true)]
    public class Hero: IAmHero
    {
        
        [ShowInInspector, ReadOnly, HideLabel, PropertyOrder(-2), HorizontalGroup("Basic")]
        public string heroName { get; set; }
        
        [ShowInInspector, PropertyOrder(-1)]
        public Health health { get; set; }
        
        [ShowInInspector, PropertyOrder(-1)]
        public Attribute attribute { get; set; }
        
        [ShowInInspector, BoxGroup("装备后的属性")]
        public Attribute equipAttribute => attribute + equipments.attribute;
        
        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup("Basic")]
        public JobSO job;
        [ReadOnly, HideLabel, HorizontalGroup("Basic")]
        public readonly CharacterSO characterSO;
        

        public IAmHero heroic => this;

        public HeroActionQueue actionQueue { get; private set; }
        
        [InlineProperty, HideLabel]
        public readonly BuffHandler buffHandler = new ();

        [ShowInInspector, InlineProperty, HideLabel, PropertyOrder(100), BoxGroup("装备后的属性")]
        public AttackPower attackPower =>  equipments.PowerCombine(job.AttributePower(equipAttribute));
        
        public bool stillAlive => !health.isEmpty;
        
        [BoxGroup("装备"), InlineProperty, HideLabel, HideReferenceObjectPicker]
        public readonly HeroEquipments equipments;
        
        Hero(Attribute a, HeroHealthStrategy healthStrategy, JobSO jobSO, CharacterSO characterSO)
        {
            this.characterSO = characterSO;
            
            attribute = a + jobSO.weaponSO.attribute;
            
            health = new Health(attribute, healthStrategy);

            job = Object.Instantiate(jobSO);
            heroName = $"[{jobSO.jobName}]{NVJOBNameGen.Uppercase(NVJOBNameGen.GiveAName(7))}";

            actionQueue = new HeroActionQueue(this);
            
            var weapon = jobSO.weaponSO.ToEquipment();

            equipments = new HeroEquipments(weapon);
            // attackPower = weapon.power + jobSO.AttributePower(attribute);

            (job.skills ?? new BuffDataSO[] {}).Select(v => v.ToBuff())
                .ForEach(v =>
            {
                buffHandler.AddBuff(v);
            });


        }

        public static Hero FromSO(CharacterSO characterSO, JobSO jobSO)
        {
            return new Hero(characterSO.attribute, characterSO.healthStrategy, jobSO, characterSO);
        }

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