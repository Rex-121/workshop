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
        
        public Inventory(IEnumerable<Slot> items, string key)
        {
            slots = items.ToList();
            saveKey = key;
        }

        public void CollectItem(IItem item)
        {
            var index = 0;

            if (!slots.IsNullOrEmpty())
            {
                foreach (var slot in slots)
                {
                    if (index < slot.index) break;
                    index = slot.index + 1;
                }
            }

            slots.Add(new Slot(index, item));
        }


        public void Remove(Inventory.Slot slot)
        {
            slots.RemoveAll(v => v.index == slot.index);
        }


        public void Save()
        {
            Storage.main.Save(saveKey, slots.ToArray());
        }
        
        
        
        [System.Serializable]
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
}