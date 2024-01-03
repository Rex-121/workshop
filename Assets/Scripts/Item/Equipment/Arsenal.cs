using Algorithm;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/Arsenal", fileName = "Arsenal")]
    public class Arsenal : SingletonSO<Arsenal>
    {

        public WeaponSO[] weaponSOs;


        public IEquipment RandomWeapon()
        {
            return weaponSOs.RandomElement().ToEquipment();
        }

    }
}
