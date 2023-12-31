using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class UIManager: MonoBehaviour
    {

        public UIManagerSO uiManagerSO;

        private KeyCode[] _allKeys;

        private GameObject _latestDisplay;

        private KeyCode _latestKey;
        
        private void Start()
        {
            _allKeys = uiManagerSO.allItems.Keys.ToArray();
        }

        private void Update()
        {
            foreach (var key in _allKeys)
            {
                if (!Input.GetKeyDown(key)) continue;
                DisplayBy(key);
                break;
            }
        }

        public void DisplayBy(KeyCode keyName)
        {
            
            DestroyPrevious();

            if (_latestKey == keyName)
            {
                _latestKey = KeyCode.RightWindows;
            }
            else
            {
                _latestKey = keyName;
                _latestDisplay = Instantiate(uiManagerSO.allItems[keyName], transform);
            }
        }

        private void DestroyPrevious()
        {
            if (!ReferenceEquals(_latestDisplay, null))
            {
                Destroy(_latestDisplay);
            }
        }
        
    }
}