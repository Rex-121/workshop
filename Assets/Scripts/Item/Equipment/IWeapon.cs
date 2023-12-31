using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public interface IWeapon: IEquipment
    {

        // public Attribute attribute { get; }
        //
        // public AttackPower power { get; }
    }


    [System.Serializable]
    public struct Sword : IWeapon
    {
        
        [SerializeField, PreviewField(60, ObjectFieldAlignment.Left), HorizontalGroup("Basic", 60), HideLabel]
        public Sprite sprite { get; set; }

        
        [SerializeField, VerticalGroup("Basic/Basic"), HideLabel]
        public string itemName { get; set; }
        
       
        [SerializeField, VerticalGroup("Basic/Basic"), HideLabel]
        public Quality quality { get; set; }
        
        [SerializeField]
        public Attribute attribute { get; set; }

        [SerializeField, VerticalGroup("Basic/Basic"), HideLabel]
        public AttackPower power { get; set; }// => new AttackPower(5, 8);

        public Sword(string name, Attribute attribute, Sprite sprite, AttackPower power, Quality qualities)
        {
            this.attribute = attribute;

            this.sprite = sprite;

            this.power = power;
            
            itemName = name;

            quality = qualities;
        }
        
        public static Sword FromSwordSO(WeaponSO weaponSO)
        {
            return new Sword(weaponSO.equipmentName, weaponSO.attribute, weaponSO.icon, new AttackPower(weaponSO.power.x, weaponSO.power.y), new Quality());
        }

        public IEquipment RemakeByQuality(IQuality qualities)
        {
            return new Sword(itemName, attribute.RemakeByQuality(qualities), sprite, power.RemakeByQuality(qualities), qualities.quality);
        }
    }
    
}