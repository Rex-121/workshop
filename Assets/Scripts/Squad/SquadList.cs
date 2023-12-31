using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class SquadList : MonoBehaviour
    {
        // Start is called before the first frame update

        public string store = "0:2-1:1-2:0";


        public SquadPlaceInList squadPrefab;


        public RectTransform panel;
        [Button]
        public void Restore()
        {
            var squads = store.Split("-");
            var aTeams = squads.Select(v => v.Split(":"));
            var squad = aTeams.Select(v => HeroGenesis.main.RestoreByIDs(v));

            var squadPlaceInList = Instantiate(squadPrefab, panel);
            squadPlaceInList.heroes = squad.ToArray();
        }
    }
}
