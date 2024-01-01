using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class SquadPlaceInList: MonoBehaviour, IPointerClickHandler
    {

        [ShowInInspector] private Hero[] _heroes;

        public Action<Hero[]> didSelect;

        public Hero[] heroes
        {
            get => _heroes;
            set
            {
                _heroes = value;
                Refresh();
            }
        }

        public HeroStands[] heroStands;

        private void Refresh()
        {
            for (var i = 0; i < heroStands.Length; i++)
            {
                heroStands[i].hero = _heroes[i];
            }
        }

        public void SquadOnAdventure()
        {
            AdventureManager.main.NewSquadOnAdventure(_heroes);
            Destroy(gameObject);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            didSelect?.Invoke(_heroes);
        }
    }
}