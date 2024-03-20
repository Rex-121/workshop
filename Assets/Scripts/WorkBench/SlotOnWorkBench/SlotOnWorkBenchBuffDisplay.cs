// using System;
// using Sirenix.OdinInspector;
// using TMPro;
// using UnityEngine;
// using WorkBench;
//
// namespace Tyrant.UI
// {
//     public class SlotOnWorkBenchBuffDisplay: MonoBehaviour
//     {
//
//         public TextMeshProUGUI buffLabel;
//         
//         public TextMeshProUGUI materialFeatureLabel;
//         
//         [ShowInInspector]
//         private DiceBuffHandler _handler2;
//         private DiceBuffHandler _handler;
//
//
//         public void NewTurnDidStarted(int arg0)
//         {
//             // _handler = new DiceBuffHandler("");
//             // _handler2 = new DiceBuffHandler("");
//         }
//
//         public void RegisterSlot(WorkBenchSlot slot)
//         {
//             _handler2 = slot.previewBuffHandler;
//             _handler = slot.buffHandler;
//             if (!ReferenceEquals(slot.materialFeature, null))
//             {
//                 materialFeatureLabel.text = slot.materialFeature.featureName;
//             }
//         }
//
//         private void Update() => buffLabel.text = $"{_handler2.AllEffect(_handler.AllEffect(0))}";
//
//     }
// }