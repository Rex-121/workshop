using UnityEngine;

namespace Tyrant
{
    
    [CreateAssetMenu(menuName = "装备/武器", fileName = "武器")]
    public class WeaponSO: EquipmentSO
    {
        public override IEquipment ToEquipment()
        {
            return Weapon.FromSwordSO(this);
        }
    }
}