using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tyrant
{
    public class SquadManager: MonoBehaviour
    {
        public static SquadManager main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                
                heroSquads.AddRange(HeroGenesis.main.GetAllSquads());
                
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }


        [NonSerialized]
        private List<HeroSquad> heroSquads = new List<HeroSquad>();


        public IEnumerable<HeroSquad> GetAllSquads() => heroSquads;


    }
}