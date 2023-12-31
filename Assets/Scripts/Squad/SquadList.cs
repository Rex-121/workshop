using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class SquadList : MonoBehaviour
    {

        public SquadPlaceInList squadPrefab;


        public RectTransform panel;
        private void Start()
        {

            var arrays = HeroGenesis.main.heroCodes;

            foreach (var store in arrays)
            {
                var squads = store.Split("-");
                var aTeams = squads.Select(v => v.Split(":"));
                var squad = aTeams.Select(v => HeroGenesis.main.RestoreByIDs(v));

                var squadPlaceInList = Instantiate(squadPrefab, panel);
                squadPlaceInList.heroes = squad.ToArray();
            }
            
           
        }
    }
}
