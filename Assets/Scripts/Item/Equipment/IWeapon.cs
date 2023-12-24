using UnityEngine;

namespace Tyrant
{
    public interface IWeapon: IEquipment
    {

        public Attribute attribute { get; }

        public AttackPower power { get; }
    }


    public struct Sword : IWeapon
    {
        public string itemName => "剑";
        public Sprite sprite { get; }
        
        public Quality quality => Quality.On(Quality.Tier.Fine);
        
        public Attribute attribute { get; set; }

        public AttackPower power => new AttackPower(5, 8);

        public Sword(Attribute attribute, Sprite sprite)
        {
            this.attribute = attribute;

            this.sprite = sprite;
        }
        
        // public Sword(EquipmentSO so)
        // {
        //     itemName = so.equipmentName;
        //     sprite = so.icon;
        // }
    }
    
}