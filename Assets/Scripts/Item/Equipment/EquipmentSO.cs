using UnityEngine;

namespace Tyrant
{
    
    [CreateAssetMenu(menuName = "装备/装备", fileName = "装备")]
    public class EquipmentSO: ScriptableObject
    {
        public string equipmentName;
        
        public Sprite icon;
    }
}