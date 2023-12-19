using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [InlineEditor]
    public interface IItem
    {
        [ShowInInspector]
        public string itemName { get; }
        
        
        [ShowInInspector]
        public Sprite sprite { get; }


        public Quality quality { get; }

    }
}
