using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class InventoryBag : MonoBehaviour
    {

        public GameObject slotPrefab;

        public Transform panel;

        public int maxSlot = 36;

        private readonly List<InventorySlotMono> _slots = new();
        
        [LabelText("用于物品拖拽的点")]
        public Transform dragPointForItem;
        private void Start()
        { 
            
            // 生成背包格子
            GenerateSlots();

            // 增加进背包
            InventoryManager.main.items.slots.Select(v => v.Value.item).ForEach(AddItem);
        }


        // 增加物品进背包
        public void AddItem(IItem item)
        {
            // 所有空格
            var allEmpty = _slots.Where(v => !v.isOccupied);
            var inventorySlots = allEmpty as InventorySlotMono[] ?? allEmpty.ToArray();
            if (!inventorySlots.Any())
            {
                // 如果没有空格
                Debug.Log("背包已无空格");
            }
            else
            {
                inventorySlots.First().AddItemIfPossible(item);
            }
            
        }


        // 生成背包格子
        private void GenerateSlots()
        {
            for (var i = 0; i < maxSlot; i++)
            {
                var gb = Instantiate(slotPrefab, panel);
                var sc = gb.GetComponent<InventorySlotMono>();
                sc.itemDraggingHandle = new ItemPreviewForInventorySlot.DefaultDragging(dragPointForItem);
                _slots.Add(sc);
            }
        }
        
    }
}
