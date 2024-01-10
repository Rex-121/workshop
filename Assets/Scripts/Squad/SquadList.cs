using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public class SquadList : MonoBehaviour
    {

        public SquadPlaceInList squadPrefab;

        public SquadAllInfoDisplay squadAllInfoDisplayPrefab;

        public RectTransform panel;

        private SquadAllInfoDisplay _latest;

        private List<HeroSquad> _squads = new();
        private void Start()
        {
            
            foreach (var heroSquad in SquadManager.main.GetAllSquads())
            {
                var squadPlaceInList = Instantiate(squadPrefab, panel);
                squadPlaceInList.heroSquad = heroSquad;
                _squads.Add(heroSquad);
                squadPlaceInList.didSelect += DidSelectSquad;
            }
            
           
        }


        // private void Update()
        // {
        //     if (_squads.All(v => v.isOnAdventure))
        //     {
        //         Destroy(gameObject);
        //     }
        // }


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
