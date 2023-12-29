using System.Collections.Generic;
using System.Linq;
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
        
        public void RegisterSlot(WorkBenchSlot slot)
        {
            // 预览buff
            slot.previewBuffs
                .CombineLatest(slot.buffs, (list, tools) => {
                    var a = new List<IToolBuff>();
                    a.AddRange(list);
                    a.AddRange(tools);
                return a;
                })
                .Subscribe(PreviewBuff)
                .AddTo(this);
            
            // 显示debuff
            DisplayDebuff(slot.debuff);
        }

        private void DisplayDebuff(IEnumerable<WorkBenchDebuff> debuffs)
        {
            if (debuffs.Count() == 0) return;
            debuffLabel.text = debuffs.First().debuffDes;
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