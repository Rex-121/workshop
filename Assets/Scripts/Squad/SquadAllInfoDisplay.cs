using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public class SquadAllInfoDisplay : MonoBehaviour
    {

        public HeroStands[] heroStands;

        public HeroInfoDisplay[] heroInfoDisplays;
        
        public void NewSquad(HeroSquad squad)
        {

            for (int i = 0; i < squad.Count; i++)
            {

                heroStands[i].hero = squad.ElementAt(i);
                heroInfoDisplays[i].hero = squad.ElementAt(i);
            }
            
        }
        
        
    }
}
