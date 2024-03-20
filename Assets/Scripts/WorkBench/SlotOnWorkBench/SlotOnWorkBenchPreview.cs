// using TMPro;
// using UnityEngine;
// using UniRx;
//
// namespace Tyrant.UI
// {
//     public class SlotOnWorkBenchPreview: MonoBehaviour
//     {
//         
//         public TextMeshProUGUI powerDisplay;
//
//         private GameObject _copyDice;
//
//         public void RegisterSlot(WorkBenchSlot slot)
//         {
//             slot.preview
//                 .Subscribe(tool =>
//                 {
//                     if (tool == null)
//                     {
//                         UnPreview();
//                     }
//                     else
//                     {
//                         Preview(tool);
//                     }
//                 })
//                 .AddTo(this);
//         }
//
//         private void UnPreview()
//         {
//             powerDisplay.text = "";
//         }
//         
//         public void NewTurnDidStarted(int arg0)
//         {
//             
//         }
//
//         private void Preview(ToolOnTable tool)
//         {
//         }
//     }
// }