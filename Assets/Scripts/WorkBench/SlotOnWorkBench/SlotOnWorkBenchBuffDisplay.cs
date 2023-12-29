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
        
        private DiceBuffHandler _handler2;
        private DiceBuffHandler _handler;
        public void RegisterSlot(WorkBenchSlot slot)
        {
            _handler2 = slot.previewBuffHandler;
            _handler = slot.buffHandler;
        }

        private void Update() => buffLabel.text = $"{_handler2.AllEffect(_handler.AllEffect(0))}";

    }
}