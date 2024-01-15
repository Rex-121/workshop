using System;
using Sirenix.OdinInspector;

namespace Tyrant.Items
{
    [Serializable, Flags]
    public enum MaterialType
    {
        [LabelText("木材")]
        Wood = 1,
        [LabelText("矿石")]
        Ore = 2,
    }
}