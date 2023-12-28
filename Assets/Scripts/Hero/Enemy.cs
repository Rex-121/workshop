using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace Tyrant
{
    public class Enemy: IAmHero
    {

        [ShowInInspector]
        public Attribute attribute { get; set; }
        
        [ShowInInspector]
        public string heroName { get; set; }
        
        public IAmHero heroic => this;
        
        [ShowInInspector]
        public Health health { get; set; }

        public bool stillAlive => !health.isEmpty;

        [ShowInInspector]
        public List<MaterialSO> loot = new();

        [ShowInInspector]
        public HeroActionQueue actionQueue { get; private set; }
        
        [ShowInInspector]
        public AttackPower attackPower;
        
        
        public BuffHandler buffHandler = new BuffHandler();

        public Enemy(EnemySO so)
        {

            var character = so.characterSO;
            
            attribute = character.attribute;
            heroName = character.characterName;
            
            loot.AddRange(so.lootSO.GetLoot());
            
            health = new Health(attribute, character.healthStrategy);
            
            actionQueue = new HeroActionQueue(this);

            attackPower = new AttackPower(5, 6);
            
            (so.skills ?? new BuffDataSO[] { })
                .Select(v => v.ToBuff())
                .ForEach(v =>
            {
                buffHandler.AddBuff(v);
            });
        }
        
        public Enemy()
        {
            attribute = new Attribute(5, 5, 5);
            health = new Health(attribute, new HeroHealthStrategy(10,10,10,50));
        }
        
        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = attackPower.power;
            
            buffHandler.UseBuffIfNeeded(v =>
            {
                v.buffDataSO.onHit?.Apply(v, attack, (newPower) =>
                {
                    attack = newPower;
                });
            });
            
            battleVersus.TakeDamage(attack);
            return attack;
        }

        public void TakeDamage(Attack attack)
        {
            health.TakeDamage(attack);
        }
    }
}