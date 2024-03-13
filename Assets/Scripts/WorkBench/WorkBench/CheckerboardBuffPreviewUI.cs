using System.Linq;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    /// <summary>
    /// 用于棋盘格显示buff预览
    /// </summary>
    public class CheckerboardBuffPreviewUI: CheckerboardBasicUI
    {
        public TextMeshProUGUI buffLabel;
        
        public override void SlotPrepared()
        {
            base.SlotPrepared();
            
            WorkBenchManager.main.checker
                .CombineLatest(WorkBenchManager.main.cardInHandStream, (wrapper, right) =>
                {
                    if (!wrapper.HasValue || right == null) return null;
                    
                    var all = right.diceBuffInfo.buffDataSO
                        .effectOnLocation
                        .AllEffect(wrapper.Value.position, WorkBenchManager.main.workBench.allSlots)
                        .Select(v => v.toolWrapper);
                    return all.Contains(slot.toolWrapper) ? right : null;
                })
                .DistinctUntilChanged()
                .Subscribe(Preview)
                .AddTo(this);
        
        }

        private void Preview(Tool tool)
        {
            if (tool == null)
            {
                Clear();
            }
            else
            {
                Display(tool.diceBuffInfo);
            }
        }

        private void Clear()
        {
            buffLabel.text = "";
        }

        private void Display(DiceBuffInfo buffInfo)
        {
            // buffLabel.count();
            buffLabel.text = buffInfo.buffDataSO.brief;
        }
    }
}