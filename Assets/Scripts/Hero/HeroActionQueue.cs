using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class HeroActionQueue
    {

        // private readonly IAmHero _hero;

        [LabelText("行动值"), SerializeField, ShowInInspector]
        private int _actionValue;

        // private Attribute attribute => _hero.attribute;


        public HeroActionQueue(IAmHero hero)
        {
            _actionValue = hero.attribute.dexterity;
        }

        [ShowInInspector, LabelText("行动积累值")]
        private int _actionStore = 0;


        public void Reset()
        {
            _actionStore = 0;
        }
        public bool CanAction(int value)
        {
            // if (!_hero.stillAlive) return false;

            _actionStore += _actionValue;

            if (_actionStore < value) return false;
            _actionStore -= value;
            return true;

        }
    }
}