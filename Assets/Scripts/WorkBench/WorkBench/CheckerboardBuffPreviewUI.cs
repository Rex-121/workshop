using System.Linq;
using TMPro;
using UniRx;

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
                    
                    // 如果此格子为buff区域
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
                Display(tool);
            }
        }

        private void Clear()
        {
            if (slot.tool != null)
            {
                buffLabel.text = "";
            }
            // buffLabel.text = "";
        }

        private void Display(Tool tool)
        {
            var value = tool.diceBuffInfo.buffDataSO.onUse.Apply(0, tool.diceBuffInfo);
            // var value = tool.diceBuffInfo.buffDataSO.onUse.Apply(0, tool.diceBuffInfo, tool, slot.buffHandler);
            buffLabel.text = $"+ {value}";
        }
    }
}