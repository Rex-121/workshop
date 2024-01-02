using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class AdventureManager: MonoBehaviour
    {

        public static AdventureManager main;

        private void Awake()
        {
            if (main == null)
            {
                main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }


        public Toggle toggle;
        public AdventureMono adventureMono;
        public bool adventurePlayFast = false;
        
        private void Start()
        {

            toggle
                .OnValueChangedAsObservable()
                .Subscribe(v =>
            {
                adventurePlayFast = v;
            }).AddTo(this);

        }


        public void NewSquadOnAdventure(HeroSquad squad)
        {
            adventureMono.NewHeroSquadOnAdventure(squad);
        }
    }
}