using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{


    public class Inventory: IDoCollectItem
    {
        public int maxSlot = 24;

        public string saveKey;

        [ShowInInspector]
        public List<Slot> slots;

        private Func<IItem, bool> _canStore;
        
        public Inventory(IEnumerable<Slot> items, string key, Func<IItem, bool> canStore)
        {
            _canStore = canStore;
            slots = items.ToList();
            saveKey = key;
        }

        public void CollectItem(IItem item)
        {
            var index = 0;

            if (!slots.IsNullOrEmpty())
            {
                foreach (var slot in slots.TakeWhile(slot => index >= slot.index))
                {
                    index = slot.index + 1;
                }
            }

            AddSlot(new Slot(index, item));
        }


        public void AddSlot(Slot slot)
        {
            if (!_canStore.Invoke(slot.item)) return;
            
            slots.Add(slot);
            
            Debug.Log($"#背包# 背包+{slot.item.itemName}-{slot.item.quality.tier}");
            
            Save();
        }


        public void RemoveSlot(Inventory.Slot slot)
        {
            if (!_canStore.Invoke(slot.item)) return;
            
            slots.RemoveAll(v => v.index == slot.index);
            
            Debug.Log($"#背包# 背包 移除 {slot.item.itemName}-{slot.item.quality.tier}");
            
            Save();
        }


        internal void Save()
        {
            Storage.main.Save(saveKey, slots.ToArray());
        }
        
        
        
        [Serializable]
        public struct Slot
        {
            [SerializeField]
            public int index;
        
            [SerializeField]
            public IItem item;

            public Slot(int index, IItem item)
            {
                this.index = index;
                this.item = item;
            }
        }
    }
    
    public static class InventoryArrayExt
    {
        public static void Save<T>(this IEnumerable<T> o) where T: Inventory
        {
                o.ForEach(v => v.Save());
        }
        
        public static void AddSlot<T>(this IEnumerable<T> o, Inventory.Slot slot) where T: Inventory
        {
            o.ForEach(v => v.AddSlot(slot));
        }
        
        public static void RemoveSlot<T>(this IEnumerable<T> o, Inventory.Slot slot) where T: Inventory
        {
            o.ForEach(v => v.RemoveSlot(slot));
        }
        
        public static void CollectItem<T>(this IEnumerable<T> o, IItem item) where T: Inventory
        {
            o.ForEach(v => v.CollectItem(item));
        }
        
        
    }
}