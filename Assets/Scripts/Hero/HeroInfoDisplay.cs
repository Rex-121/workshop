using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UniRx;
namespace Tyrant
{
    public class HeroInfoDisplay: MonoBehaviour
    {

        private Hero _hero;

        private IDisposable _infoDis;
        public Hero hero
        {
            set
            {
                _hero = value;
                _infoDis?.Dispose();
                _infoDis = hero?.heroDidChange
                    .Subscribe(v => Refresh())
                    .AddTo(this);
                
                Refresh();
            }
            get => _hero;
        }


        public EquipmentDoEquipment weaponEquip;

        public Image jobIcon;

        public TextMeshProUGUI nameLabel, sLabel, dLabel, iLabel, attackPowerLabel;

        public TMP_InputField nameInputField;
        
        // public EquipmentBriefDisplay equipmentBriefDisplays;

        private void Refresh()
        {
            if (ReferenceEquals(hero, null)) return;


         
            

            if (!ReferenceEquals(weaponEquip, null))
            {
                weaponEquip.equipments = hero.equipments;
            }

            if (!ReferenceEquals(nameInputField, null))
            {
                nameInputField.text = hero.heroName;
            }
            
            if (!ReferenceEquals(nameLabel, null))
            {
                nameLabel.text = hero.heroName;
            }
            
            
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
            

            // if (!ReferenceEquals(equipmentBriefDisplays, null))
            // {
            //     equipmentBriefDisplays.Equipment(hero.equipments.weapon);
            // }
           
        }


        public void OnNameChanged()
        {
            hero.heroName = nameInputField.text;
        }
    }
}