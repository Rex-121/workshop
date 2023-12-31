using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [InlineEditor]
    public interface IItem
    {
        public string itemName { get; }
        
        public Sprite sprite { get; }

        public Quality quality { get; }

    }
}
