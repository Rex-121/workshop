using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public class SquadAllInfoDisplay : MonoBehaviour
    {

        public HeroStands[] heroStands;
        
        public void NewSquad(HeroSquad squad)
        {

            for (var i = 0; i < squad.Count; i++)
            {

                heroStands[i].hero = squad.ElementAt(i);
            }
            
        }
        
        
    }
}
