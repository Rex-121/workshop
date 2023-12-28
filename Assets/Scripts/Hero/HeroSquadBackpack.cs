using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
namespace Tyrant
{
        public class HeroSquadBackpack : MonoBehaviour
        {
    
            public InventorySlot slotPrefab;
    
            public Transform panel;
    
            public int maxSlot = 8;

            public int occupied = 0;
    
            private readonly List<InventorySlot> _slots = new();

            public TextMeshProUGUI occupiedLabel;
            
            [LabelText("用于物品拖拽的点")]
            public Transform dragPointForItem;
            private void Start()
            { 
                
                
                
                // RefreshOccupiedLabelIfNeeded();
            }
            
            private void Awake()
            {
                // 生成背包格子
                GenerateSlots();
                
            }

            // 增加物品进背包
            public void AddItem(IItem item)
            {

                GenerateSlots();
                
                // 所有空格
                var allEmpty = _slots.Where(v => !v.isOccupied);
                var inventorySlots = allEmpty as InventorySlot[] ?? allEmpty.ToArray();
                if (!inventorySlots.Any())
                {
                    // 如果没有空格
                    Debug.Log("背包已无空格");
                }
                else
                {
                    inventorySlots.First().AddItemIfPossible(item);
                    occupied = maxSlot - inventorySlots.Length + 1;
                }

                RefreshOccupiedLabelIfNeeded();
            }

            private void RefreshOccupiedLabelIfNeeded()
            {
                if (!ReferenceEquals(occupiedLabel, null))
                {
                    occupiedLabel.text = $"{occupied}/{maxSlot}";
                }
            }
    
    
            // 生成背包格子
            private void GenerateSlots()
            {
                if (_slots.IsNullOrEmpty())
                {
                    for (var i = 0; i < maxSlot; i++)
                    {
                        _slots.Add(Instantiate(slotPrefab, panel));
                    }
                }
                
                RefreshOccupiedLabelIfNeeded();
            }
            
        }
    
}