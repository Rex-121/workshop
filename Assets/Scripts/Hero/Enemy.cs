using System.Collections.Generic;
using Sirenix.OdinInspector;

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

        public Enemy(EnemySO so)
        {
            attribute = so.attribute;
            heroName = so.enemyName;
            
            loot.AddRange(so.lootSO.GetLoot());
            
            health = new Health(attribute, so.healthStrategy);
            
            actionQueue = new HeroActionQueue(this);

            attackPower = new AttackPower(5, 6);
        }
        
        public Enemy()
        {
            attribute = new Attribute(5, 5, 5);
            health = new Health(attribute, new HeroHealthStrategy(10,10,10,50));
        }
        
        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = attackPower.power;
            battleVersus.TakeDamage(attack);
            return attack;
        }

        public void TakeDamage(Attack attack)
        {
            health.TakeDamage(attack);
        }
    }
}