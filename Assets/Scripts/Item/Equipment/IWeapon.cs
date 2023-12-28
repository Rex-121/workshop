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


    public struct Sword : IWeapon
    {
        [ShowInInspector]
        public string itemName { get; set; }
        
        [ShowInInspector]
        public Sprite sprite { get; }
        
        [ShowInInspector]
        public Quality quality => Quality.On(Quality.Tier.Fine);
        
        [ShowInInspector]
        public Attribute attribute { get; set; }

        [ShowInInspector]
        public AttackPower power { get; set; }// => new AttackPower(5, 8);

        public Sword(string name, Attribute attribute, Sprite sprite, AttackPower power)
        {
            this.attribute = attribute;

            this.sprite = sprite;

            this.power = power;
            this.itemName = name;
        }
        
        public static Sword FromSwordSO(WeaponSO weaponSO)
        {
            return new Sword(weaponSO.name, weaponSO.attribute, weaponSO.icon, new AttackPower(weaponSO.power.x, weaponSO.power.y));
        }
        
        // public Sword(EquipmentSO so)
        // {
        //     itemName = so.equipmentName;
        //     sprite = so.icon;
        // }
    }
    
}