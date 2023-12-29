using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using Tools;
using UnityEngine;
using UniRx;

namespace Tyrant.UI
{
    public class SlotOnWorkBenchBuffDisplay: MonoBehaviour
    {

        public TextMeshProUGUI buffLabel;
        
        public TextMeshProUGUI debuffLabel;

        private DiceBuffHandler _handler2;
        private DiceBuffHandler _handler;
        public void RegisterSlot(WorkBenchSlot slot)
        {
            // 预览buff
            // slot.previewBuffs
            //     .CombineLatest(slot.buffs, (list, tools) => {
            //         var a = new List<IToolBuff>();
            //         a.AddRange(list);
            //         a.AddRange(tools);
            //     return a;
            //     })
            //     .Subscribe(PreviewBuff)
            //     .AddTo(this);
            _handler2 = slot.previewBuffHandler;
            _handler = slot.buffHandler;
            // 显示debuff
            // DisplayDebuff(slot.debuff);
        }

        private void Update()
        {
            // buffLabel.text = $"{_handler.buffs.FirstOrDefault()?.currentStack}-{_handler2.buffs.FirstOrDefault()?.currentStack}";
            // var b = 0;
            // var a = _handler.buffs
            //     .Sum(diceBuffInfo => b = diceBuffInfo.buffDataSO.onPreview?.Apply(b, diceBuffInfo) ?? b);
            // // var c = _handler2.buffs
            // //     .Sum(diceBuffInfo => a = diceBuffInfo.buffDataSO.onPreview?.Apply(a, diceBuffInfo) ?? a);
            //
            // // var k = 0;
            // _handler2.buffs.ForEach(v =>
            // {
            //     var ff = v.buffDataSO.onPreview?.Apply(a, v) ?? 0;
            //     a = ff;
            // });

            var c = _handler2.AllEffect(_handler.AllEffect(0));

            // c = _handler2.AllEffectByFace(_handler.AllEffectByFace(c));
            //
            // Debug.Log($"{a}");
            // //
            // // var d = c + a;
            //
            buffLabel.text = $"{c}";
        }

        // private void DisplayDebuff(IEnumerable<WorkBenchDebuff> debuffs)
        // {
        //     if (debuffs.Count() == 0) return;
        //     debuffLabel.text = debuffs.First().debuffDes;
        // }
        
        // private void PreviewBuff(IEnumerable<DiceBuffInfo> simpleBuffTool)
        // {
        //     buffLabel.text = simpleBuffTool.Sum(v => v.ValueBy(0)).ToString();
        // }

        // private void UnSlot()
        // {
        //     buffLabel.text = "";
        // }
        
    }
}