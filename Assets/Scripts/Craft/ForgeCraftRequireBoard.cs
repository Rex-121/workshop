using System;
using System.Collections.Generic;
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
        private void Start()
        {
            for (int i = 0; i < requires; i++)
            {
                var gb = Instantiate(slotPrefab, panel).GetComponent<InventorySlot>();

                gb.handler = new MyStruct(item =>
                {
                    gb.AddItem(item.item);
                    item.Clear();
                });

                gb.ItemDraggingHandle = new ItemPreviewForInventorySlot.DefaultDragging(anchor);
            }
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
                Debug.Log("OnDropOnDropOnDropOnDrop");
                v.Invoke(item);
            }
        }
        
    }
}