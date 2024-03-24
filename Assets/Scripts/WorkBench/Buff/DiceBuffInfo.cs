using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class DiceBuffInfo
    {
        [ShowInInspector]
        public Guid id = Guid.NewGuid();

        public DiceBuffInfo(DiceBuffDataSO s)
        {
            buffDataSO = s;
        }

        
        public readonly DiceBuffDataSO buffDataSO;

        [ShowInInspector]
        public int diceFace;

        public Sprite icon => buffDataSO.icon;
        
    }
    
        
    public static class BuffEffectOnExt
    {
        /// <summary>
        /// 所有收到影响的slot
        /// </summary>
        /// <param name="on">buff</param>
        /// <param name="slot">slot</param>
        /// <returns></returns>
        public static IEnumerable<Vector2Int> EffectedOnSlots(this DiceBuffDataSO.IBuffEffectOn on, WorkBenchSlot slot)
        {
            if (on == null)
                return new[] {slot.toolWrapper.position};
            return on.effectOnSlot.Select(v => slot.toolWrapper.position + v);
        }
    }
}