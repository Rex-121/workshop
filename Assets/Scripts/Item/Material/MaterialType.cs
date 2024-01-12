using System;
using Sirenix.OdinInspector;

namespace Tyrant.Items
{
    [Serializable]
    public enum MaterialType
    {
        [LabelText("木材")]
        Wood,
        [LabelText("矿石")]
        Ore,
    }
}