using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class InventoryBag : MonoBehaviour
    {

        public GameObject slotPrefab;

        public Transform panel;

        public int maxSlot = 36;

        private List<InventorySlot> _slots = new();
        

        public Transform anchorx;
        private void Start()
        { 
            GenerateSlots();
        }


        public void AddItem(IItem item)
        {
            var emptySlot = _slots.First(v => !v.isOccupied);
            emptySlot.AddItem(item);
        }


        private void GenerateSlots()
        {
            for (var i = 0; i < maxSlot; i++)
            {
                var gb = Instantiate(slotPrefab, panel);
                var sc = gb.GetComponent<InventorySlot>();
                sc.ItemDraggingHandle = new ItemPreviewForInventorySlot.DefaultDragging(anchorx);
                _slots.Add(sc);
            }
        }
        
    }
}
