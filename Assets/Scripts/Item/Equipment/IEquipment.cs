using UnityEngine;

namespace Tyrant
{
    public interface IEquipment: IItem
    {
        
    }
    
    
    
    
    public struct Equipment: IEquipment
    {
        public string itemName { get; }
        
        
        public Sprite sprite { get; set; }

        public Quality quality => Quality.On(Quality.Tier.Fine);

        // public Equipment(string name)
        // {
        //     itemName = name;
        //     sprite = null;
        // }


        public Equipment(EquipmentSO so)
        {
            itemName = so.equipmentName;
            sprite = so.icon;
        }
    }
    
    
}