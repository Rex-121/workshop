using UniRx;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tyrant
{
        public class HeroSquadBackpackMono : MonoBehaviour
        {
    
            [FormerlySerializedAs("slotPrefab")] public InventorySlotMono slotMonoPrefab;
    
            public Transform panel;
            
            public int occupied = 0;
    
            private readonly List<InventorySlotMono> _slots = new();

            public TextMeshProUGUI occupiedLabel;


            private SquadInventory _squadInventory;
            public SquadInventory squadInventory
            {
                set
                {
                    _squadInventory = value;
                    Refresh();
                }
                get => _squadInventory;
            }
            private void Refresh()
            {
                
                // 生成背包格子
                GenerateSlots(squadInventory.maxSlot);
                
                squadInventory.items.Subscribe(v =>
                {
                    _slots.ForEach(a => a.Clear());
                    v.ForEach(AddItem);
                }).AddTo(this);
            }

            // 增加物品进背包
            private void AddItem(IItem item)
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
                    occupied = squadInventory.maxSlot - inventorySlots.Length + 1;
                }

                RefreshOccupiedLabelIfNeeded();
            }

            private void RefreshOccupiedLabelIfNeeded()
            {
                if (!ReferenceEquals(occupiedLabel, null))
                {
                    occupiedLabel.text = $"{occupied}/{squadInventory.maxSlot}";
                }
            }
    
    
            // 生成背包格子
            private void GenerateSlots(int maxSlot)
            {
                if (_slots.IsNullOrEmpty())
                {
                    for (var i = 0; i < maxSlot; i++)
                    {
                        _slots.Add(Instantiate(slotMonoPrefab, panel));
                    }
                }
                
                RefreshOccupiedLabelIfNeeded();
            }
            
        }
    
}