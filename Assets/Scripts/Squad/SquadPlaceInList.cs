using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class SquadPlaceInList: MonoBehaviour, IPointerClickHandler
    {

        [ShowInInspector] private HeroSquad _heroSquad;

        public Action<HeroSquad> didSelect;

        public Button goAdventureButton;

        public HeroSquad heroSquad
        {
            get => _heroSquad;
            set
            {
                _heroSquad = value;
                Refresh();
            }
        }

        public HeroStands[] heroStands;

        private void Refresh()
        {
            for (var i = 0; i < heroStands.Length; i++)
            {
                heroStands[i].hero = heroSquad.heroes.ElementAt(i);
            }
        }

        public void SquadOnAdventure()
        {
            if (heroSquad.isOnAdventure) return;
            AdventureManager.main.NewSquadOnAdventure(heroSquad);
            Destroy(gameObject);
        }

        private void Update()
        {
            // 禁用按钮
            goAdventureButton.interactable = !heroSquad.isOnAdventure;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            didSelect?.Invoke(heroSquad);
        }
    }
}