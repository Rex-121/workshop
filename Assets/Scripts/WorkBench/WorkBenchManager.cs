using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tools;
using Tyrant.UI;
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

        // public WorkBenchUI workBenchUI;


        public void PreviewTool(ToolOnTable toolOnTable, Vector2Int location)
        {

            var slot = workBench.SlotBy(location);

            slot.PreviewTool(toolOnTable.tool);

            var pin = new PinedTool(location, toolOnTable.tool);
            
            var buffTools = pin.buffs.Select(v => v.simpleBuffTool).ToArray();
            
            buffTools
                .Where(v => workBench.HasSlot(v.position))
                .ForEach(v =>
                {
                    workBench.SlotBy(v.position).NewPreviewBuff(v);
                });
        }

        public void Pin(Vector2Int location, ToolOnTable toolOnTable)
        {
            var slot = workBench.SlotBy(location);

            slot.Pin(toolOnTable);
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
        }

    }
}
