using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    // [CreateAssetMenu(menuName = "装备/装备", fileName = "装备")]
    public class EquipmentSO: SerializedScriptableObject
    {
        
        public Guid id = Guid.NewGuid();
        
        public string equipmentName;
        
        public Sprite icon;

        public Attribute attribute;

        
        public Vector2Int power;


        public virtual IEquipment ToEquipment() => null;
    }
}