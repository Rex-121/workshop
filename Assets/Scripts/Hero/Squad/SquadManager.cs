using System;
using System.Collections.Generic;
using Sirenix.Utilities;
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

                _heroSquads.AddRange(SaveManager.main.LoadSquads());
                
            }
            else
            {
                Destroy(this);
            }
        }


        public void Save()
        {
            SaveManager.main.SaveSquads();
        }


        [NonSerialized]
        private readonly List<HeroSquad> _heroSquads = new List<HeroSquad>();


        public IEnumerable<HeroSquad> GetAllSquads() => _heroSquads;


    }
}