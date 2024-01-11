using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UniRx;

namespace Tyrant
{
    [HideReferenceObjectPicker, InlineProperty, HideLabel, BoxGroup("HERO", centerLabel: true)]
    public class Hero: IAmHero
    {

        [ReadOnly, HideLabel, SerializeField, PropertyOrder(-2), HorizontalGroup("Basic")]
        public string heroName { get; set; }
        
        [SerializeField, PropertyOrder(-1)]
        public Health health { get; set; }
        
        [PropertyOrder(-1), SerializeField]
        public Attribute attribute { get; set; }
        
        [ShowInInspector, BoxGroup("装备后的属性")]
        public Attribute equipAttribute => attribute + equipments.attribute;
        
        [ShowInInspector, ReadOnly, HideLabel, HorizontalGroup("Basic")]
        public Job job;

        [ReadOnly, HideLabel, HorizontalGroup("Basic")]
        public CharacterSO characterSO => HeroGenesis.main.FindCharacterByID(_characterId);

        [SerializeField]
        private int _characterId;

        public IAmHero heroic => this;

        [SerializeField]
        public HeroActionQueue actionQueue { get; private set; }
        
        [InlineProperty, HideLabel]
        public BuffHandler buffHandler = new ();

        [ShowInInspector, InlineProperty, HideLabel, PropertyOrder(100), BoxGroup("装备后的属性")]
        public AttackPower attackPower =>  equipments.PowerCombine(job.AttributePower(equipAttribute));
        
        public bool stillAlive => !health.isEmpty;
        
        [BoxGroup("装备"), InlineProperty, HideLabel, HideReferenceObjectPicker]
        public HeroEquipments equipments;

        // public Hero()
        // {
        //     
        // }
        
        Hero(Attribute a, HeroHealthStrategy healthStrategy, JobSO jobSO, CharacterSO characterSO)
        {
            // this.characterSO = characterSO;

            _characterId = characterSO.id;
            
            attribute = a + jobSO.weaponSO.attribute;
            
            health = new Health(attribute, healthStrategy);

            job = jobSO.ToJob();//Object.Instantiate(jobSO);
            heroName = $"[{job.jobName}]{NVJOBNameGen.Uppercase(NVJOBNameGen.GiveAName(7))}";

            actionQueue = new HeroActionQueue(this);
            
            // var weapon = jobSO.weaponSO.ToEquipment();

            equipments = new HeroEquipments();
            // attackPower = weapon.power + jobSO.AttributePower(attribute);

            (jobSO.skills ?? new BuffDataSO[] {}).Select(v => v.ToBuff())
                .ForEach(v =>
            {
                buffHandler.AddBuff(v);
            });
        }

        public void Restore()
        {
            (job.jobSO.skills ?? new BuffDataSO[] {})
                .Select(v => v.ToBuff())
                .ForEach(buffHandler.AddBuff);
            
            // equipments.Restore();
            
            equipments
                .needSave
                .Skip(1)
                .Subscribe(v => Save());
        }


        public void Save()
        {
            SquadManager.main.Save();
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