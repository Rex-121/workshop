using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tyrant
{
    public class HeroInfoDisplay: MonoBehaviour
    {

        private IAmHero _hero;
        public IAmHero hero
        {
            set
            {
                _hero = value;
                Refresh();
            }
            get => _hero;
        }


        public TextMeshProUGUI nameLabel, sLabel, dLabel, iLabel;


        private void Refresh()
        {
            nameLabel.text = _hero.heroName;
            sLabel.text = $"Strength {_hero.attribute.strength}";
            dLabel.text = $"Dexterity {_hero.attribute.dexterity}";
            iLabel.text = $"Intelligence {_hero.attribute.intelligence}";
        }
    }
}