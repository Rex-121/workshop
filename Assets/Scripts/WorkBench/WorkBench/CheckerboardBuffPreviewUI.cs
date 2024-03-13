using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant
{
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
                    return all.Contains(slot.toolWrapper) ? right.diceBuffInfo : null;
                })
                .DistinctUntilChanged()
                .Subscribe(Preview);

        }

        private void Preview(DiceBuffInfo diceBuffInfo)
        {
            if (diceBuffInfo == null)
            {
                Clear();
            }
            else
            {
                Display(diceBuffInfo);
            }
        }

        private void Clear()
        {
            buffLabel.text = "";
        }

        private void Display(DiceBuffInfo buffInfo)
        {
            buffLabel.text = buffInfo.buffDataSO.brief;
        }
    }
}