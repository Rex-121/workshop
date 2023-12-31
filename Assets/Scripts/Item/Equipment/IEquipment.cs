using UnityEngine;

namespace Tyrant
{

    public interface IRemakeByQuality<T>
    {
        public T RemakeByQuality(IQuality qualities);

    }
    
    public interface IEquipment: IItem, IRemakeByQuality<IEquipment>
    { 
        public Attribute attribute { get; }
        public AttackPower power { get; }


        // public IEquipment RemakeByQuality(IQuality make, IQuality quality);

    }
    
    
    
    
    // public struct Equipment: IEquipment
    // {
    //     public string itemName { get; }
    //     
    //     
    //     public Sprite sprite { get; set; }
    //
    //     public Quality quality => Quality.On(Quality.Tier.Fine);
    //
    //     // public Equipment(string name)
    //     // {
    //     //     itemName = name;
    //     //     sprite = null;
    //     // }
    //
    //
    //     public Equipment(EquipmentSO so)
    //     {
    //         itemName = so.equipmentName;
    //         sprite = so.icon;
    //     }
    // }
    
    
}