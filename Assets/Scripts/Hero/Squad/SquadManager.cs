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

                var data = Storage.main.Load<HeroSquad[]>("SQUAD");
                data.ForEach(v => v.Restore());
                
                _heroSquads.AddRange(data);
                
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }


        public void Save()
        {
            Storage.main.SaveSquad(_heroSquads.ToArray());
        }


        [NonSerialized]
        private readonly List<HeroSquad> _heroSquads = new List<HeroSquad>();


        public IEnumerable<HeroSquad> GetAllSquads() => _heroSquads;


    }
}