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


        public EquipmentDoEquipment weaponEquip;

        public Image jobIcon;

        public TextMeshProUGUI nameLabel, sLabel, dLabel, iLabel, attackPowerLabel;

        public EquipmentBriefDisplay equipmentBriefDisplays;

        private void Refresh()
        {
            if (ReferenceEquals(hero, null)) return;

            weaponEquip.equipments = hero.equipments;
            
            nameLabel.text = hero.heroName;
            sLabel.text = $"{hero.equipAttribute.strength}";
            dLabel.text = $"{hero.equipAttribute.dexterity}";
            iLabel.text = $"{hero.equipAttribute.intelligence}";

            if (!ReferenceEquals(jobIcon, null))
            {
                jobIcon.sprite = hero.job.icon;
            }

            if (!ReferenceEquals(attackPowerLabel, null))
            {
                attackPowerLabel.text = hero.attackPower.predictPower;
            }
            

            if (!ReferenceEquals(equipmentBriefDisplays, null))
            {
                equipmentBriefDisplays.Equipment(hero.equipments.weapon);
            }
           
        }
    }
}