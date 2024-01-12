using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "UI/UIManagerSO")]
    public class UIManagerSO: SerializedScriptableObject
    {
 
        
        public struct Wrapper
        {

            public bool isOnCanvas;

            public GameObject gameObject;

            public Wrapper(GameObject gameObject, bool isOnCanvas)
            {
                this.gameObject = gameObject;
                this.isOnCanvas = isOnCanvas;
            }
        }
        
        public Dictionary<KeyCode, Wrapper> allUIItems = new();
        
        private Dictionary<KeyCode, GameObject> allItems = new();


    }
}