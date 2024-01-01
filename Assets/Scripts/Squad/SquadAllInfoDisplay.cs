using UnityEngine;

namespace Tyrant
{
    public class SquadAllInfoDisplay : MonoBehaviour
    {

        public HeroStands[] heroStands;

        public HeroInfoDisplay[] heroInfoDisplays;
        
        public void NewSquad(Hero[] heroes)
        {

            for (int i = 0; i < heroes.Length; i++)
            {

                heroStands[i].hero = heroes[i];
                heroInfoDisplays[i].hero = heroes[i];
            }
            
        }
        
        
    }
}
