using System;

namespace Tyrant
{
    public class Health
    {

        public int totalHealth;

        public int currentHealth;

        public string healthBarDisplay => $"{currentHealth}/{totalHealth}";
        public Health(Attribute attribute, HeroHealthStrategy heroHealthStrategy)
        {
            totalHealth = heroHealthStrategy.Health(attribute);
            currentHealth = totalHealth;
        }

        public bool isEmpty => currentHealth <= 0;

        /// <summary>
        /// 扣血
        /// </summary>
        /// <param name="damage">伤害</param>
        /// <returns></returns>
        public int TakeDamage(int damage)
        {
            currentHealth = Math.Max(0, currentHealth - damage);
            return currentHealth;
        }
        
        public int TakeDamage(Attack attack)
        {
            return TakeDamage(attack.power);
        }
        
    }
}