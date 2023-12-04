using System;
using System.Collections.Generic;
using System.Linq;
using Dicing;
using Sirenix.OdinInspector;
using Tools;
using Tyrant.UI;
using UniRx;
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
        public Dictionary<ToolWrapper, IDicing> dic = new();

        public BehaviorSubject<int> make = new(0);
        public BehaviorSubject<int> quality = new(0);
        
        public struct ToolWrapper
        {
            public Vector2Int position;
            public SlotType type;

            public ToolWrapper(Vector2Int position, SlotType type)
            {
                this.position = position;
                this.type = type;
            }
        }
        
        public List<ToolWrapper> Start()
        {
            
            int[][] a = new int[3][]
            {
                new int[5] {0, 0, 0, 1, 0},
                new int[5] {1, 2, 1, 2, 1},
                new int[5] {0, 0, 0, 1, 0},
            };

            var list = new List<ToolWrapper>();
            
            for (var i = a.Length - 1; i >= 0; i--)
            {
                var b = a[i];
                for (var j = 0; j < b.Length; j++)
                {
                    var position = new Vector2Int(i, j);
                    var slotType = b[j].toSlotType();
                    var item = new ToolWrapper(position, slotType);
                    if (slotType != SlotType.Empty)
                    {
                        dic.Add(item, null);
                    }
                    list.Add(item);
                }
            }

            return list;
        }
        
        
        
        public void DidPinTool(Vector2Int index, Tool tool)
        {
            var key = dic.Keys.First(v => v.position == index);
            dic[key] = tool.dice;

           Calculate();
        }

        public void DidUnPinTool(Vector2Int index, Tool tool)
        {
            var key = dic.Keys.First(v => v.position == index);
            dic[key] = null;
            
            Calculate();
        }

        private void Calculate()
        {
            var makeDice = dic.Keys.Where(v => v.type == SlotType.Make);

            var value1 = makeDice
                .Select(v => dic[v])
                .Where(v => v != null)
                .Sum(v => v.Roll());
            
            make.OnNext(value1);
            
            var value2 = dic.Keys.Where(v => v.type == SlotType.Quality)
                .Select(v => dic[v])
                .Where(v => v != null)
                .Sum(v => v.Roll());
            
            quality.OnNext(value2);
        }

        public bool CanBePlaced(ToolOnTable toolOnTable)
        {
            return true;
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