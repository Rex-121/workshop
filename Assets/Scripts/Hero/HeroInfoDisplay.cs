using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tyrant
{
    public class HeroInfoDisplay: MonoBehaviour
    {

        private Hero _hero;
        public Hero hero
        {
            set
            {
                _hero = value;
                Refresh();
            }
            get => _hero;
        }


        public Image jobIcon;

        public TextMeshProUGUI nameLabel, sLabel, dLabel, iLabel, attackPowerLabel;

        public EquipmentBriefDisplay equipmentBriefDisplays;

        private void Refresh()
        {
            nameLabel.text = _hero.heroName;
            sLabel.text = $"{_hero.attribute.strength}";
            dLabel.text = $"{_hero.attribute.dexterity}";
            iLabel.text = $"{_hero.attribute.intelligence}";

            if (!ReferenceEquals(jobIcon, null))
            {
                jobIcon.sprite = hero.job.icon;
            }

            attackPowerLabel.text = hero.attackPower.predictPower;

            if (!ReferenceEquals(equipmentBriefDisplays, null))
            {
                equipmentBriefDisplays.Equipment(hero.weapon);
            }
           
        }
    }
}