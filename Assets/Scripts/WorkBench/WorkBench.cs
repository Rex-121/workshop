using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant.UI;
using UnityEngine;

namespace Tyrant
{
    public class WorkBench
    {
        [Serializable]
        public enum SlotType
        {
            Empty = 0, Make = 1, Quality = 2
        }
        
        
        [ShowInInspector, NonSerialized]
        private Dictionary<ToolWrapper, WorkBenchSlot> _dic = new();

        public Dictionary<ToolWrapper, WorkBenchSlot> allSlots => _dic;

        public BluePrint bluePrint;
        
        public WorkBench(BluePrint bluePrint)
        {
            this.bluePrint = bluePrint;
        }
        
        [HideReferenceObjectPicker]
        public struct ToolWrapper
        {
            [ReadOnly, LabelText("位置"), HorizontalGroup("Info")]
            public Vector2Int position;
            
            [SuffixLabel("Slot类型", true), HideLabel, HorizontalGroup("Info")]
            public SlotType type;

            public ToolWrapper(Vector2Int position, SlotType type)
            {
                this.position = position;
                this.type = type;
            }
        }
        
        public IEnumerable<WorkBenchSlot> allMakes => _dic.Values
            .Where(v => v.toolWrapper.type == WorkBench.SlotType.Make);
        public IEnumerable<WorkBenchSlot> allQuality => _dic.Values
            .Where(v => v.toolWrapper.type == WorkBench.SlotType.Quality);
        
        // 获取所有受影响的Slot
        public IEnumerable<WorkBenchSlot> GetAllEffectPositions(Vector2Int location, ToolOnTable toolOnTable)
        {
            return toolOnTable.diceBuffInfo.buffDataSO.effectOnLocation.AllEffect(location, allSlots);
        }

        public WorkBenchSlot SlotBy(Vector2Int vector2Int)
        {
            return _dic.FirstOrDefault(v => v.Key.position == vector2Int).Value;
        }

        public void NewBuffTo(Vector2Int position, ToolOnTable toolOnTable)
        {
            GetAllEffectPositions(position, toolOnTable)
                .ForEach(v => v.NewBuff(toolOnTable.diceBuffInfo));
        }
        
        public void NewPreviewBuffTo(Vector2Int position, ToolOnTable toolOnTable)
        {
            GetAllEffectPositions(position, toolOnTable)
                .ForEach(v => v.NewPreviewBuff(toolOnTable.diceBuffInfo));
        }

        public void DidForgeThisTurn()
        {
            _dic.Values.ForEach(v => v.DidForgeThisTurn());
        }


        private List<WorkBenchSlot> LockBluePrint(BluePrint bluePrint)
        {
            var requires = bluePrint.boardLines;

            // panel.constraintCount = requires.First().Count();
            
            var list = new List<WorkBenchSlot>();
            
            for (var i = requires.Count() - 1; i >= 0; i--)
            {
                var b = requires.ElementAt(i);
                for (var j = 0; j < b.Count(); j++)
                {
                    var position = new Vector2Int(i, j);
                    var slotType = b.ElementAt(j).toSlotType();
                    var item = new ToolWrapper(position, slotType);
                    var slot = new WorkBenchSlot(item);
                    if (slotType != SlotType.Empty)
                    {
                        _dic.Add(item, slot);
                    }

                    list.Add(slot);
                }
            }

            return list;
        }

        public List<WorkBenchSlot> Start()
        {
            return LockBluePrint(this.bluePrint);
        }
    }

    public static class IntExt
    {
        public static WorkBench.SlotType toSlotType(this int o)
        {
            return o switch
            {
                1 => WorkBench.SlotType.Make,
                2 => WorkBench.SlotType.Quality,
                _ => WorkBench.SlotType.Empty
            };
        }
    }
}