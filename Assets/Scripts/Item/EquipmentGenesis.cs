using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/EquipmentGenesis", fileName = "EquipmentGenesis")]
    public class EquipmentGenesis: SingletonSO<EquipmentGenesis>
    {

        public EquipmentSO equipmentSO;

        
        public IEquipment DoCraft(int makes, int quality)
        {
            return null;
            // return new Sword(new Attribute(5,5, 5), equipmentSO.icon);
        }

    }
}