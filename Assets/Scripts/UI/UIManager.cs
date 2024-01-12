using System;
using System.Linq;
using Algorithm;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class UIManager: MonoBehaviour
    {

        public static UIManager main;

        private void Awake()
        {
            if (main == null)
            {
                main = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public UIManagerSO uiManagerSO;

        private KeyCode[] _allKeys;

        private GameObject _latestDisplay;

        private KeyCode _latestKey;

        public Transform dragPointForItem;

        public Canvas canvas;
        // 用于查看道具属性
        public Transform inspectorTransform;
        public EquipmentInspector inspectorPrefab;
        [NonSerialized]
        private EquipmentInspector _inspector;
        public void InspectorItem(IItem item)
        {
            if (item == null)
            {
                DestroyInspector();
            }
            else
            {
                if (_inspector == null)
                {
                    _inspector = Instantiate(inspectorPrefab, inspectorTransform);
                    _inspector.equipment = item as IEquipment;
                    _inspector.transform.position = Input.mousePosition;
                }
                inspectorTransform.SetAsLastSibling();
            }
        }
        
        private void Start()
        {
            _allKeys = uiManagerSO.allUIItems.Keys.ToArray();
        }

        private void Update()
        {
            foreach (var key in _allKeys)
            {
                if (!Input.GetKeyDown(key)) continue;
                DisplayBy(key);
                break;
            }

            if (!ReferenceEquals(_inspector, null))
            {
                _inspector.transform.position = Input.mousePosition;
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

                var item = uiManagerSO.allUIItems[keyName];

                _latestDisplay = item.isOnCanvas ? Instantiate(item.gameObject, transform) : Instantiate(item.gameObject);

                if (_latestDisplay.TryGetComponent(out InventoryBag bag))
                {
                    bag.dragPointForItem = dragPointForItem;
                }
            }
            
            
            dragPointForItem.SetAsLastSibling();
        }


        private void DestroyInspector()
        {
            if (_inspector == null) return;
            Destroy(_inspector.gameObject);
            _inspector = null;
        }

        private void DestroyPrevious()
        {
            if (!ReferenceEquals(_latestDisplay, null))
            {
                Destroy(_latestDisplay);
            }

            DestroyInspector();
        }
        
    }
}