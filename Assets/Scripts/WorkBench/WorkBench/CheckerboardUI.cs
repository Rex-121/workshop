using System;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class CheckerboardUI: CheckerboardBasicUI, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        // [ShowInInspector]
        // public WorkBenchSlot slot { get; private set; }


        private BehaviorSubject<CardInfoMono> card = new BehaviorSubject<CardInfoMono>(null);


        public override void SetSlot(WorkBenchSlot workBenchSlot)
        {
            
            SetName(workBenchSlot);
            
            GetComponent<CheckerboardToolPreviewUI>().SetSlot(workBenchSlot);

            base.SetSlot(workBenchSlot);
        }

        /// <summary>
        /// 面板名称
        /// </summary>
        private void SetName(WorkBenchSlot workBenchSlot)
        {
            name = workBenchSlot.name;
        }


        public override void SlotPrepared()
        {

           
            
            var board = WorkBenchManager.main.checker
                .Where(v => v.HasValue)
                .Where(v => v!.Value == slot.toolWrapper);
            
            board
                .Select(v => WorkBenchManager.main.cardInHand)
                .Where(v => v != null)
                .Subscribe(v =>
                {
                    Debug.Log($"#SLOT# 进入[{slot.name}]");
                    
                    // 1. 检视自己
                    var ui = GetComponent<CheckerboardDisplayUI>();
                    ui.SetV(v);
                    
                }).AddTo(this);


           
            // card.Subscribe(v =>
            // {
            //     if (ReferenceEquals(v, null)) return;
            //    
            //     // Debug.Log($"#SLOT# 进入[{slot.name}]");
            //     
            //     
            //     Debug.Log(WorkBenchManager.main.workBench.allSlots);
            // });
        }

        /// <summary>
        /// 此slot是否可以执行事件
        /// </summary>
        /// <returns></returns>
        private bool CanSlotTypeExecuteEvent()
        {
            return slot.toolWrapper.type != WorkBench.SlotType.Empty;
        }
        
        public void OnDrop(PointerEventData eventData)
        {

            var obj = eventData.pointerDrag.gameObject;

            if (!obj.TryGetComponent(out CardInfoMono cardInfoMono))
            {
                return;
            }

            Debug.Log(cardInfoMono.tool);
            
            cardInfoMono.Use();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            GetCardByEventData(eventData);


            // Debug.Log(cardInfoMono.tool);
            //
            // var d = cardInfoMono.tool.diceBuffInfo.buffDataSO.effectOnLocation;
            // // d.ef
            // Debug.Log(d);

        }

        private void GetCardByEventData(PointerEventData eventData)
        {
            if (!eventData.dragging) return;
            
            
            WorkBenchManager.main.EnterCheckerboard(slot.toolWrapper);
            
            // var obj = eventData.pointerDrag.gameObject;
            //
            // if (!obj.TryGetComponent(out CardInfoMono cardInfoMono))
            // {
            //     return;
            // }
            //
            // card.OnNext(cardInfoMono);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            WorkBenchManager.main.EnterCheckerboard(null);
            // Debug.Log(eventData.pointerDrag);
        }
    }
}