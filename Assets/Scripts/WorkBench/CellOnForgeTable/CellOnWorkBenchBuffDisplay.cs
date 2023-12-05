using System;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using Tools;
using UnityEngine;
using UniRx;

namespace Tyrant.UI
{
    public class CellOnWorkBenchBuffDisplay: MonoBehaviour
    {

        public TextMeshProUGUI buffLabel;
        public void RegisterSlot(WorkBenchSlot slot)
        {
            slot.buffs
                .Subscribe(tool =>
                {
                    if (tool.IsNullOrEmpty())
                    {
                        UnSlot();
                    }
                    else
                    {
                        PreviewBuff(tool.First());
                    }
                })
                .AddTo(this);
        }
        public void PreviewBuff(SimpleBuffTool simpleBuffTool)
        {
            buffLabel.text = simpleBuffTool.dice.Roll().ToString();
        }

        public void UnSlot()
        {
            buffLabel.text = "";
        }
        
    }
}