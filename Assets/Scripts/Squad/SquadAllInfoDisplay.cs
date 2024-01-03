using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public class SquadAllInfoDisplay : MonoBehaviour
    {

        public HeroStands[] heroStands;
        
        public void NewSquad(HeroSquad squad)
        {
            var heroes = squad.heroes.ToArray();

            for (var i = 0; i < heroes.Length; i++)
            {

                heroStands[i].hero = heroes.ElementAt(i);
            }
            
        }
        
        
    }
}
