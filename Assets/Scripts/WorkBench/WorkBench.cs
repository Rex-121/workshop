using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class WorkBench: IWorkBenchRound
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

        public Material[] materials;
        
        public WorkBench(BluePrint bluePrint, IMaterial[] material)
        {
            this.bluePrint = bluePrint;

            materials = material.OfType<Material>().ToArray();
        }
        
        [HideReferenceObjectPicker]
        public struct ToolWrapper: WorkBenchManager.ICheckerStatus
        {
            [ReadOnly, HideLabel, HorizontalGroup("Info")]
            public Vector2Int position;
            
            [SuffixLabel("Slot类型", true), HideLabel, HorizontalGroup("Info")]
            public SlotType type;

            public ToolWrapper(Vector2Int position, SlotType type)
            {
                this.position = position;
                this.type = type;
            }
            
            public static bool operator == (ToolWrapper a, ToolWrapper b)
            {
                return a.position == b.position && a.type == b.type;
            }

            public static bool operator !=(ToolWrapper a, ToolWrapper b)
            {
                return !(a == b);
            }

            public string debugDescription => $"棋盘格 {position}";
        }
        
        public IEnumerable<WorkBenchSlot> allMakes => _dic.Values
            .Where(v => v.toolWrapper.type == WorkBench.SlotType.Make);
        public IEnumerable<WorkBenchSlot> allQuality => _dic.Values
            .Where(v => v.toolWrapper.type == WorkBench.SlotType.Quality);
        
        public WorkBenchSlot SlotBy(Vector2Int vector2Int)
        {
            return _dic.FirstOrDefault(v => v.Key.position == vector2Int).Value;
        }

        private List<WorkBenchSlot> LockBluePrint(BluePrint bluePrint)
        {
            var requires = bluePrint.boardLines;
            
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

            var listx = new List<MaterialFeatureSO>();

            materials.ForEach(v =>
            {
                listx.AddRange(v.features.Select(ScriptableObject.Instantiate));
            });

            MaterialFeatureMakes.From(listx.ToArray(), _dic.Values);

            return list;
        }

        public List<WorkBenchSlot> Start()
        {
            return LockBluePrint(this.bluePrint);
        }

        public void PrepareNewRound()
        {
            
        }

        public void DidEndRound()
        {
            
        }

        public void NewTurn()
        {
            _dic.Values.ForEach(v => v.DidForgeThisTurn());
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