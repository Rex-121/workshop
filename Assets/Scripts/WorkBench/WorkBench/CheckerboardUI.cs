using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant
{
    public class CheckerboardUI: CheckerboardBasicUI, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        
        public Image backgroundImage;
        
        public override void SetSlot(WorkBenchSlot workBenchSlot)
        {

            backgroundImage.enabled = workBenchSlot.toolWrapper.type != WorkBench.SlotType.Empty;

            SetName(workBenchSlot);
            
            GetComponent<CheckerboardToolPreviewUI>().SetSlot(workBenchSlot);

            GetComponent<CheckerboardDisplayUI>().SetSlot(workBenchSlot);
            
            GetComponent<CheckboardMaterialUI>().SetSlot(workBenchSlot);

            base.SetSlot(workBenchSlot);
        }

        /// <summary>
        /// 面板名称
        /// </summary>
        private void SetName(WorkBenchSlot workBenchSlot)
        {
            name = workBenchSlot.name;
        }
        
        public void OnDrop(PointerEventData eventData)
        {

            var obj = eventData.pointerDrag.gameObject;

            if (!obj.TryGetComponent(out CardInfoMono cardInfoMono))
            {
                return;
            }

            WorkBenchManager.main.UseToolOnSlot(cardInfoMono, slot);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            GetCardByEventData(eventData);
        }

        private void GetCardByEventData(PointerEventData eventData)
        {
            // if (!eventData.dragging) return;
            WorkBenchManager.main.EnterCheckerboard(WorkBenchManager.CheckerStatus<WorkBench.ToolWrapper>.Enter(slot.toolWrapper));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // if (!eventData.dragging) return;
            WorkBenchManager.main.EnterCheckerboard(WorkBenchManager.CheckerStatus<WorkBench.ToolWrapper>.Leave(slot.toolWrapper));
        }
    }
}