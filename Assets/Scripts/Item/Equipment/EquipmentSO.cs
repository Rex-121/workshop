using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    // [CreateAssetMenu(menuName = "装备/装备", fileName = "装备")]
    public class EquipmentSO: SerializedScriptableObject
    {
        [ReadOnly]
        [SerializeField]
        [Title("基本信息")]
        [HorizontalGroup("Basic")]
        [HideLabel]
        [VerticalGroup("Basic/Info")]
        public Guid id;
        
        [HideLabel]
        [VerticalGroup("Basic/Info")]
        public string equipmentName;
        
        [PreviewField(60)]
        [HideLabel]
        [HorizontalGroup("Basic", 60), PropertyOrder(-10)]
        public Sprite icon;

        
        [VerticalGroup("Basic/Info"), LabelText("攻击力")]
        public Vector2Int power;

        
        [BoxGroup("属性")]
        public Attribute attribute;


        public virtual IEquipment ToEquipment() => null;


        [Button]
        public void AssignGuid()
        {
            id = Guid.NewGuid();
        } 
    }
}