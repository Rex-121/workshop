using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/EquipmentGenesis", fileName = "EquipmentGenesis")]
    public class EquipmentGenesis: SingletonSO<EquipmentGenesis>
    {

        [Sirenix.OdinInspector.ReadOnly]
        public WeaponSO[] weaponSos;

        [ShowInInspector]
        public IEquipment before;
        
        
        [ShowInInspector]
        public IEquipment remake;
        
        public IEquipment DoCraft(IQuality make, IQuality quality, EquipmentSO equipmentSO)
        {

            before = equipmentSO.ToEquipment();
            
            
            var q = new QualityGroup(make, new IQuality[] {quality}, new NormalMakeWithQualityStrategy());


            remake = before.LiftByQuality(q);

            return remake;
        }

        public WeaponSO FindWeaponSOById(string id)
        {
            return weaponSos.First(v => v.id.ToString() == id);
        }
        
#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateEquipmentSO()
        {
            weaponSos = AssetDatabase.FindAssets("t:WeaponSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<WeaponSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif

    }
}