using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class SquadList : MonoBehaviour
    {

        public SquadPlaceInList squadPrefab;

        public SquadAllInfoDisplay squadAllInfoDisplayPrefab;

        public RectTransform panel;

        private SquadAllInfoDisplay _latest;
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


                squadPlaceInList.didSelect += DidSelectSquad;
            }
            
           
        }


        private void DidSelectSquad(Hero[] heroes)
        {

            if (!ReferenceEquals(_latest, null))
            {
                Destroy(_latest.gameObject);
            }
            
            _latest = Instantiate(squadAllInfoDisplayPrefab, transform);
            _latest.NewSquad(heroes);
        }
    }
}
