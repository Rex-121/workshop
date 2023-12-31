using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "UI/UIManagerSO")]
    public class UIManagerSO: SerializedScriptableObject
    {
 
        
        public Dictionary<KeyCode, GameObject> allItems = new();


    }
}