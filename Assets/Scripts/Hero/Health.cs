using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tyrant
{
    [HideReferenceObjectPicker, HideLabel, InlineProperty, Serializable]
    public class Health
    {
        [SerializeField]
        private int totalHealth;

        // [ProgressBar(0, "totalHealth", ColorGetter = "GetHealthBarColor", Height = 30), ShowInInspector, HideLabel]
        [SerializeField] private int _currentHealth;
        // {
        //     set => currentHealth.Value = Math.Min(totalHealth, value);
        //     get => currentHealth.Value;
        // }

        private Color GetHealthBarColor(float value)
        {
            return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / totalHealth, 2));
        }
        
        private string healthDisplay => $"{_currentHealth}/{totalHealth}";

        [ShowInInspector]
        private ReactiveProperty<int> currentHealth
        {
            get
            {
                if (_currentHealthRx == null)
                {
                    _currentHealthRx = new ReactiveProperty<int>(_currentHealth);
                }

                return _currentHealthRx;
            }
        }

        private ReactiveProperty<int> _currentHealthRx;
        [HideInInspector]
        public ReadOnlyReactiveProperty<string> healthBarDisplay => currentHealth.Select(_ => healthDisplay).ToReadOnlyReactiveProperty();
        //         }
        //
        //         return _healthBarDisplay;
        //     }
        // }
        //
        // ReadOnlyReactiveProperty<string> _healthBarDisplay;
        
        public Health(Attribute attribute, HeroHealthStrategy heroHealthStrategy)
        {
            totalHealth = heroHealthStrategy.Health(attribute);
            _currentHealth = totalHealth;

            // healthBarDisplay = currentHealth.Select(_ => healthDisplay).ToReadOnlyReactiveProperty();
        }

        public bool isEmpty => _currentHealth <= 0;


        /// <summary>
        /// 扣血
        /// </summary>
        /// <param name="damage">伤害</param>
        /// <returns></returns>
        public int TakeDamage(int damage)
        {
            
            _currentHealth = Math.Min(totalHealth, Math.Max(0, _currentHealth - damage));
            currentHealth.Value = _currentHealth;
            return _currentHealth;
        }
        
        public int TakeDamage(Attack attack)
        {
            return TakeDamage(attack.damage);
        }
        
    }
}