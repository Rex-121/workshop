using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [System.Serializable]
    public class Loot<T>
    {
        
            /// the object to return
            public T loot;
            
            public float Weight = 1f;

            [HideInInspector]
            public float ChancePercentage;

            [LabelText("概率"), ShowInInspector] public string ChancePercentageString => $"{ChancePercentage}%";
            
            public float RangeFrom { get; set; }
            
            public float RangeTo { get; set; }
            
    }
}