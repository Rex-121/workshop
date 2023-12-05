using System.Collections.Generic;
using Tools;
using Tyrant.UI;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class WorkBenchSlot
    {
        
        public WorkBench.ToolWrapper toolWrapper;

        public WorkBenchSlot(WorkBench.ToolWrapper toolWrapper)
        {

            this.toolWrapper = toolWrapper;

            buffs = new BehaviorSubject<List<SimpleBuffTool>>(buffTools);
        }

        public List<SimpleBuffTool> buffTools = new();


        public BehaviorSubject<Tool> preview = new(null);
        public BehaviorSubject<List<SimpleBuffTool>> buffs;

        public BehaviorSubject<GameObject> pined = new(null);
        public void Pin(ToolOnTable toolOnTable)
        {
            pined.OnNext(toolOnTable.gameObject);
        }
        public void PreviewTool(Tool tool)
        {
            preview.OnNext(tool);
        }

        public void NewPreviewBuff(SimpleBuffTool buffTool)
        {
            buffTools.Add(buffTool);
            buffs.OnNext(buffTools);
        }
        
        public void ReleasePreviewBuff()
        {
            buffTools.Clear();
            buffs.OnNext(buffTools);
            UnPreviewTool();
        }

        public void UnPreviewTool()
        {
            preview.OnNext(null);
        }
    }
}