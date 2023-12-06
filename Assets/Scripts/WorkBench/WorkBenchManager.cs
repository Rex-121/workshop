using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tools;
using Tyrant.UI;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class WorkBenchManager : MonoBehaviour
    {
        #region 单例

        

       
        public static WorkBenchManager main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        
        #endregion
        
        [ShowInInspector, NonSerialized] public WorkBench workBench = new WorkBench();

        #region 制作过程

        

        
        // public WorkBenchUI workBenchUI;
        public readonly BehaviorSubject<int> make = new(0);
        public readonly BehaviorSubject<int> quality = new(0);

        public void PreviewTool(ToolOnTable toolOnTable, Vector2Int location)
        {

            var slot = workBench.SlotBy(location);
            
            if (slot.isOccupied) return;

            slot.PreviewTool(toolOnTable.tool);

            var pin = new PinedTool(location, toolOnTable.tool);
            
            pin.buffs
                .Where(v => workBench.HasSlot(v.effectOnLocation))
                .ForEach(v =>
                {
                    workBench.SlotBy(v.effectOnLocation).NewPreviewBuff(v);
                });
            
        }

        public void Pin(Vector2Int location, ToolOnTable toolOnTable)
        {
            
            var slot = workBench.SlotBy(location);

            if (slot.isOccupied) return;
            
                        
            // 先取消预览
            UnPreviewTool(location);
            
            slot.Pin(toolOnTable);

            var pin = new PinedTool(location, toolOnTable.tool);
            
            pin.buffs
                .Where(v => workBench.HasSlot(v.effectOnLocation))
                .ForEach(v =>
                {
                    workBench.SlotBy(v.effectOnLocation).NewBuff(v);
                });
            
            CalculateScore();
            
        }

        private int allMakesScore => workBench.allMakes
            .Select(v => v.CalculateScore())
            .Sum();

        private int allQualityScore => workBench.allQuality
            .Select(v => v.CalculateScore())
            .Sum();
        
        private void CalculateScore()
        {
            make.OnNext(allMakesScore);
            quality.OnNext(allQualityScore);
        }
        

        public void UnPin(Vector2Int location)
        {
            workBench.SlotBy(location).UnPin();
            
            CalculateScore();
        }

        public void UnPreviewTool(Vector2Int location)
        {
            workBench.SlotBy(location).UnPreviewTool();

            workBench.dic.Values.ForEach(v => v.ReleasePreviewBuff());
        }


        public void Drag(ToolOnTable toolOnTable)
        {
            if (toolOnTable == null)
            {
                // 清空所有的preview
                workBench.dic.Values.ForEach(v => v.ReleasePreviewBuff());
            }
            else
            {
                // 清空所有的buff
                workBench.dic.Values.ForEach(v => v.ReleaseBuffBy(toolOnTable.tool.id));
            }
        }
        #endregion


        public void DidForgeThisTurn()
        {
            workBench.DidForgeThisTurn();
            CalculateScore();
        }
        
    }
}
