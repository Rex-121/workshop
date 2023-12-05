using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using Tools;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;

namespace Tyrant.UI
{
    public class CellOnWorkBenchBuffDisplay: MonoBehaviour
    {

        public TextMeshProUGUI buffLabel;
        public void RegisterSlot(WorkBenchSlot slot)
        {
            slot.previewBuffs
                .CombineLatest(slot.buffs, (list, tools) => {
                    var a = new List<IToolBuff>();
                    a.AddRange(list);
                    a.AddRange(tools);
                return a;
                })
                .Subscribe(PreviewBuff)
                .AddTo(this);
        }
        private void PreviewBuff(IEnumerable<IToolBuff> simpleBuffTool)
        {
            buffLabel.text = simpleBuffTool.Sum(v => v.ValueBy(0)).ToString();
        }

        private void UnSlot()
        {
            buffLabel.text = "";
        }
        
    }
}