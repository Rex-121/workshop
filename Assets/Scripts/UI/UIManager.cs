using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Tyrant.UI;
using UnityEngine.SceneManagement;

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

        public AnimationCurve animationCurve;

        public UIManagerSO uiManagerSO;

        private KeyCode[] _allKeys;

        private GameObject _latestDisplay;

        private KeyCode _latestKey;

        public Transform dragPointForItem;

        public Canvas canvas;


        [Button]
        public void D()
        {

            var perLevel = 100;

            var maxLevel = 5;
            
            for (int i = 0; i < 5; i++)
            {
                var d = animationCurve.Evaluate(i * 1.0f / maxLevel) + 1;
                Debug.Log(d * perLevel);
            }
        }
        
        [BoxGroup("Inspectors"), LabelText("检查器图层")]
        // 用于查看道具属性
        public Transform inspectorTransform;
        [BoxGroup("Inspectors"), LabelText("装备")]
        public ItemInspector equipsInspectorPrefab;
        [BoxGroup("Inspectors"), LabelText("材料")]
        public ItemInspector materialInspectorPrefab;
        [NonSerialized]
        private ItemInspector _inspector;
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
                    switch (item)
                    {
                        case IEquipment:
                            _inspector = Instantiate(equipsInspectorPrefab, inspectorTransform);
                            break;
                        case IMaterial:
                            _inspector = Instantiate(materialInspectorPrefab, inspectorTransform);
                            break;
                    }
                    
                    _inspector.NewItem(item);
                    _inspector.transform.position = Input.mousePosition;
                }
                inspectorTransform.SetAsLastSibling();
            }
        }
        
        public void PinInspectorItem(IItem item)
        {
            if (item == null)
            {
                DestroyInspector();
            }
            else
            {
                if (_inspector == null)
                {
                    switch (item)
                    {
                        case IEquipment:
                            _inspector = Instantiate(equipsInspectorPrefab);
                            break;
                        case IMaterial:
                            _inspector = Instantiate(materialInspectorPrefab);
                            break;
                    }
                    
                    _inspector.NewItem(item);
                    // _inspector.transform.position = Input.mousePosition;
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
            
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene("CardScene", LoadSceneMode.Additive);
                // SceneManager.UnloadSceneAsync()
            }
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                // SceneManager.LoadScene("CardScene", LoadSceneMode.Additive);
                var d = SceneManager.UnloadSceneAsync("CardScene");
                d.allowSceneActivation = true;
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