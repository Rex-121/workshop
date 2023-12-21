using System;
using UnityEngine;

namespace Tyrant
{
    [RequireComponent(typeof(Hero))]
    public class HeroActionQueue: MonoBehaviour
    {

        private Hero _hero;

        private Attribute attribute => _hero.attribute;


        private void Awake()
        {
            _hero = GetComponent<Hero>();
        }

        public int actionStore = 0;

        public Hero CanAction(int value)
        {
            
            actionStore += attribute.dexterity;

            if (actionStore < value) return null;
            actionStore -= value;
            return _hero;

        }
    }
}