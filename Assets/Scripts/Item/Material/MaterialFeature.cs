using System;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using UnityEngine;
using UnityEngine.Events;

namespace Tyrant
{


    public static class MaterialFeatureMakes
    {
        public static void From(MaterialFeatureSO[] so, IEnumerable<WorkBenchSlot> slots)
        {
            var l = new List<Vector2Int>();

            for (int i = 0; i < so.Count(); i++)
            {
                var slot = c(l, slots);
                var d = so.ElementAt(i);
                slot.materialFeature = ScriptableObject.Instantiate(d);
            }
        }

        private static WorkBenchSlot c(List<Vector2Int> l, IEnumerable<WorkBenchSlot> slots)
        {
            var slot = slots.RandomElement();
            if (l.Contains(slot.toolWrapper.position)) return c(l, slots);
            l.Add(slot.toolWrapper.position);
            return slot;
        }

        /// <summary>
        /// 展示所有名称
        /// </summary>
        public static string DebugDescription<T>(this T o) where T: IEnumerable<MaterialFeatureSO>
        {
            return o.Select(v => v.featureName)
                .JoinWithSeparator("-");
        }
        
    }

    
    // TODO: !!!!!!!!!!!!
    public struct X : IMaterialFeature
    {
        public Tuple<bool, int> ApplyDice(int dice)
        {
            return dice > 3 ? new Tuple<bool, int>(true, dice) : new Tuple<bool, int>(false, 0);
        }
    }

    public struct BuffForbid : IMaterialFeature
    {
        public Tuple<bool, int> ApplyDice(int dice)
        {
            return new Tuple<bool, int>(false, dice);
        }
    }

    public interface IMaterialFeature
    {
        // (是否满足条件，满足条件后的值)
        public Tuple<bool, int> ApplyDice(int dice);
    }
}