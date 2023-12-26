using System;
using UniRx;

namespace Tyrant
{
    public class Health
    {

        public int totalHealth;

        public int currentHealth
        {
            set
            {
                _currentHealth = Math.Min(totalHealth, value);
            }
            get => _currentHealth;
        }

        private int _currentHealth;
        
        public BehaviorSubject<string> healthDisplayRx = new BehaviorSubject<string>("");

        public string healthBarDisplay => $"{currentHealth}/{totalHealth}";
        public Health(Attribute attribute, HeroHealthStrategy heroHealthStrategy)
        {
            totalHealth = heroHealthStrategy.Health(attribute);
            currentHealth = totalHealth;


            Refresh();
        }

        public bool isEmpty => currentHealth <= 0;

        private void Refresh()
        {
            healthDisplayRx.OnNext(healthBarDisplay);
        }

        /// <summary>
        /// 扣血
        /// </summary>
        /// <param name="damage">伤害</param>
        /// <returns></returns>
        public int TakeDamage(int damage)
        {
            currentHealth = Math.Max(0, currentHealth - damage);
            Refresh();
            return currentHealth;
        }
        
        public int TakeDamage(Attack attack)
        {
            return TakeDamage(attack.power);
        }
        
    }
}