using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    [HideReferenceObjectPicker, HideLabel, InlineProperty,]
    public class Health
    {
        private readonly int _totalHealth;

        [ProgressBar(0, "_totalHealth", ColorGetter = "GetHealthBarColor", Height = 30), ShowInInspector, HideLabel]
        private int _currentHealth
        {
            set => currentHealth.Value = Math.Min(_totalHealth, value);
            get => currentHealth.Value;
        }

        private Color GetHealthBarColor(float value)
        {
            return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / _totalHealth, 2));
        }
        
        private string healthDisplay => $"{_currentHealth}/{_totalHealth}";
        
        private readonly ReactiveProperty<int> currentHealth = new ReactiveProperty<int>(0);

        [HideInInspector]
        public readonly ReadOnlyReactiveProperty<string> healthBarDisplay;
        public Health(Attribute attribute, HeroHealthStrategy heroHealthStrategy)
        {
            _totalHealth = heroHealthStrategy.Health(attribute);
            _currentHealth = _totalHealth;

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