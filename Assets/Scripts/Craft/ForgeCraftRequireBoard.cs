using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class ForgeCraftRequireBoard: MonoBehaviour
    {


        public GameObject slotPrefab;
        
        public int requires = 2;

        public Transform panel;

        [LabelText("拖拽点")]
        public Transform anchor;

        public BluePrintSO bluePrintSO;

        public BluePrint bluePrint;

        public List<InventorySlot> slots = new();
        private void Start()
        {

            bluePrint = BluePrint.FromSO(bluePrintSO);

            var requires = bluePrint.rawMaterialsRequires;
            
            for (int i = 0; i < requires.Count(); i++)
            {
                var gb = Instantiate(slotPrefab, panel).GetComponent<InventorySlot>();
                
                gb.AddRequire(requires.ElementAt(i));

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
        }

        private void Check()
        {
            var materials = slots.Where(v => v.previewItem != null)
                .Select(v => v.previewItem.item as IMaterial);
            
            var isEnough = bluePrint.IsMaterialEnough(materials);
            Debug.Log($"材料是否齐备 {isEnough}");
        }
        

        private class MyStruct: InventorySlot.IInventorySlotDrag
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