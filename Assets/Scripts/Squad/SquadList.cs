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
            
            foreach (var heroSquad in HeroGenesis.main.GetAllSquads())
            {
                var squadPlaceInList = Instantiate(squadPrefab, panel);
                squadPlaceInList.heroSquad = heroSquad;
                
                squadPlaceInList.didSelect += DidSelectSquad;
            }
            
           
        }


        private void DidSelectSquad(HeroSquad squad)
        {

            if (!ReferenceEquals(_latest, null))
            {
                Destroy(_latest.gameObject);
            }
            
            _latest = Instantiate(squadAllInfoDisplayPrefab, transform);
            _latest.NewSquad(squad);
        }
    }
}
