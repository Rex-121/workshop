using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class ForgeCraftRequireBoard: MonoBehaviour
    {


        public GameObject slotPrefab;
        public GameObject emptySlotPrefab;
        
        public GridLayoutGroup panel;

        [LabelText("拖拽点")]
        public Transform anchor;
        
        public List<InventorySlotMono> slots = new();
        
        private static BluePrint bluePrint => RequestManager.main.bluePrint;
        private void Start()
        {
            
            var requires = bluePrint.boardLines;

            panel.constraintCount = requires.First().Count();
            
            for (int i = 0; i < requires.Count(); i++)
            {

                var line = requires.ElementAt(i);

                line.ForEach(v =>
                {
                    if (v == 0)
                    {
                        Instantiate(emptySlotPrefab, panel.transform);
                    }
                    else
                    {
                        var gb = Instantiate(slotPrefab, panel.transform).GetComponent<InventorySlotMono>();
                    
                        // gb.AddRequire(requires.ElementAt(i));
                    
                        gb.handler = new MyStruct(item =>
                        {
                            var success = gb.AddItemIfPossible(item.item);
                            if (success)
                            {
                                item.Clear();
                                Check();
                            }
                        });
                    
                        slots.Add(gb);
                    
                        gb.itemDraggingHandle = new ItemPreviewForInventorySlot.DefaultDragging(anchor);
                    }
                   

                });

                
            }
        }

        private void Check()
        {
            var materials = slots.Where(v => v.previewItem != null)
                .Select(v => v.previewItem.item as IMaterial);
            
            var isEnough = bluePrint.IsMaterialEnough(materials);
            Debug.Log($"材料是否齐备 {isEnough}");
        }
        

        private class MyStruct: InventorySlotMono.IInventorySlotDrag
        {
            private Action<ItemPreviewForInventorySlot> v;

            public MyStruct(Action<ItemPreviewForInventorySlot> v)
            {
                this.v = v;
            }
            public void OnDrop(ItemPreviewForInventorySlot item)
            {
                v.Invoke(item);
            }
        }
        
    }
}