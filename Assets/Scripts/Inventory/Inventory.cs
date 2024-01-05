using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Tyrant
{


    public class Inventory: IDoCollectItem
    {
        public int maxSlot = 24;

        public string saveKey;

        [ShowInInspector]
        public Dictionary<int, Slot> slots;

        private Func<IItem, bool> _canStore;

        private ReplaySubject<Dictionary<int, Slot>> _slotsRx = new ();

        // 获取当前的物品
        public IObservable<Slot> SlotBy(int index) => _slotsRx
            // .Where(v => v.ContainsKey(index))
            .Select(v =>
            {
                if (v.TryGetValue(index, out var expression)) return expression;
                return new Slot(-1, null);
            });
        
        public Inventory(Dictionary<int, Slot> items, string key, Func<IItem, bool> canStore)
        {
            _canStore = canStore;
            slots = items;//.ToList();
            saveKey = key;
            _slotsRx.OnNext(items);
        }

        public void CollectItem(IItem item)
        {
            // 如果不是此背包类型
            if (!_canStore.Invoke(item)) return;
            
            var index = 0;

            // if (slots.Count != 0)
            // {
            //     foreach (var slot in slots.Keys.TakeWhile(slot => index >= slot))
            //     {
            //         index = slot + 1;
            //     }
            // }

            while (slots.ContainsKey(index))
            {
                index++;
            }
            
            Debug.Log(index);

            AddSlot(new Slot(index, item));
        }

        public void SwapSlot(Slot a, Slot? b, int toIndex)
        {
            // 移除原先的
            RemoveSlot(a, false);
            
            if (b == null)
            {
                // 如果没有可交换的，直接将物品添加到指定位置
                AddSlot(new Slot(toIndex, a.item), false);
            }
            else
            {
                // 交换
                var c = b.Value!;
                RemoveSlot(b.Value!, false);
                AddSlot(new Slot(a.index, c.item), false);
                AddSlot(new Slot(c.index, a.item), false);
            }
            
            Save();
        }

        public void Replace(Inventory.Slot slot, IItem item)
        {
            RemoveSlot(slot, false);
            AddSlot(new Slot(slot.index, item));
        }

        public void AddSlot(Slot slot, bool save = true)
        {
            if (!_canStore.Invoke(slot.item)) return;
            
            slots.Add(slot.index, slot);
            
            Debug.Log($"#背包# 背包{slot.index} 增加++{slot.item.itemName}-{slot.item.quality.tier}");
            
            if (save)
            {
                Save();
            }
        }


        public void RemoveSlot(Inventory.Slot slot, bool save = true)
        {
            if (!_canStore.Invoke(slot.item)) return;
            
            slots.Remove(slot.index);
            
            Debug.Log($"#背包# 背包{slot.index} 移除--{slot.item.itemName}-{slot.item.quality.tier}");
            
            if (save)
            {
                Save();
            }
        }


        internal void Save()
        {
            
            _slotsRx.OnNext(slots);
            
            Storage.main.Save(saveKey, slots);
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
        
        public static void SwapSlot<T>(this IEnumerable<T> o, Inventory.Slot a, Inventory.Slot? b, int toIndex) where T: Inventory
        {
            o.ForEach(v => v.SwapSlot(a, b, toIndex));
        }


        public static void Replace<T>(this IEnumerable<T> o, Inventory.Slot slot, IItem item) where T : Inventory =>
            o.ForEach(v => v.Replace(slot, item));

    }
}