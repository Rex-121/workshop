using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/EquipmentGenesis", fileName = "EquipmentGenesis")]
    public class EquipmentGenesis: SingletonSO<EquipmentGenesis>
    {

        // public EquipmentSO equipmentSO;

        [ShowInInspector]
        public IEquipment before;
        
        
        [ShowInInspector]
        public IEquipment remake;
        
        public IEquipment DoCraft(IQuality make, IQuality quality, EquipmentSO equipmentSO)
        {

            // Debug.Log($"#Forge# {makes} - {quality}");
            before = equipmentSO.ToEquipment();
            
            
            var q = new QualityGroup(make, new IQuality[] {quality}, new NormalMakeWithQualityStrategy());


            remake = before.RemakeByQuality(q);

            return remake;
            // return new Sword(new Attribute(5,5, 5), equipmentSO.icon);
        }

    }
}