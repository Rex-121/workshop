using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class SquadPlaceInList: MonoBehaviour
    {

        [ShowInInspector] private Hero[] _heroes;

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
        

    }
}