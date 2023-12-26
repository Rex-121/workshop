using System;
using UniRx;

namespace Tyrant
{
    public class Health
    {

        public readonly int totalHealth;

        private int _currentHealth
        {
            set => currentHealth.Value = Math.Min(totalHealth, value);
            get => currentHealth.Value;
        }
        
        public string healthDisplay => $"{_currentHealth}/{totalHealth}";
        
        public readonly ReactiveProperty<int> currentHealth = new ReactiveProperty<int>(0);

        public readonly ReadOnlyReactiveProperty<string> healthBarDisplay;
        public Health(Attribute attribute, HeroHealthStrategy heroHealthStrategy)
        {
            totalHealth = heroHealthStrategy.Health(attribute);
            _currentHealth = totalHealth;

            healthBarDisplay = currentHealth.Select(_ => healthDisplay).ToReadOnlyReactiveProperty();
        }

        public bool isEmpty => _currentHealth <= 0;


        /// <summary>
        /// 扣血
        /// </summary>
        /// <param name="damage">伤害</param>
        /// <returns></returns>
        public int TakeDamage(int damage)
        {
            _currentHealth = Math.Max(0, _currentHealth - damage);
            return _currentHealth;
        }
        
        public int TakeDamage(Attack attack)
        {
            return TakeDamage(attack.damage);
        }
        
    }
}