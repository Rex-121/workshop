using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Tyrant.UI
{
    [CreateAssetMenu(menuName = "MessageChannel/材料检查", fileName = "材料检查MC")]
    public class ItemInspectorMessageChannel: SerializedScriptableObject
    {
        
        public UnityAction<IItem> itemInspector;
        
        
        public void ItemInspector(IItem item) => itemInspector?.Invoke(item);
    }
}