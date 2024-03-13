using System;
using System.Linq;
using TMPro;
using UniRx;

namespace Tyrant
{
    public class CheckerboardToolPreviewUI: CheckerboardBasicUI
    {
        public TextMeshProUGUI toolLabel;

        private CheckerboardBuffPreviewUI _buffPreviewUI;

        private void Awake()
        {
            _buffPreviewUI = GetComponent<CheckerboardBuffPreviewUI>();
        }

        public override void SetSlot(WorkBenchSlot workBenchSlot)
        {

            _buffPreviewUI.SetSlot(workBenchSlot);
            
            base.SetSlot(workBenchSlot);
        }

        public override void SlotPrepared()
        {
            base.SlotPrepared();
            
            WorkBenchManager.main.checker
                .CombineLatest(WorkBenchManager.main.cardInHandStream, (wrapper, right) =>
                {
                    if (!wrapper.HasValue || right == null) return null;

                    return wrapper.Value != slot.toolWrapper ? null : right;
                })
                .DistinctUntilChanged()
                .Subscribe(Preview).AddTo(this);

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
            toolLabel.text = "";
        }

        private void Display(Tool tool)
        {
            toolLabel.text = tool.dice.Roll().ToString();
        }
    }
}