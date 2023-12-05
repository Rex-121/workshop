using Dicing;
using TMPro;
using Tools;
using UnityEngine;
using UniRx;

namespace Tyrant.UI
{
    public class CellOnWorkBenchPreview: MonoBehaviour
    {
        
        public TextMeshProUGUI powerDisplay;

        private GameObject _copyDice;

        public void RegisterSlot(WorkBenchSlot slot)
        {
            slot.preview
                .Subscribe(tool =>
                {
                    if (tool == null)
                    {
                        UnPreview();
                    }
                    else
                    {
                        Preview(tool);
                    }
                })
                .AddTo(this);
        }

        private void UnPreview()
        {
            powerDisplay.text = "";
        }

        private void Preview(Tool tool)
        {
            powerDisplay.text = tool.dice.Roll().ToString();
        }
    }
}