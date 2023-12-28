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
            if (ReferenceEquals(hero, null)) return;
            
            nameLabel.text = hero.heroName;
            sLabel.text = $"{hero.attribute.strength}";
            dLabel.text = $"{hero.attribute.dexterity}";
            iLabel.text = $"{hero.attribute.intelligence}";

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