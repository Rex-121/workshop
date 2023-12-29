using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tools;
using Tyrant.UI;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class WorkBenchSlot
    {
        
        [HideLabel, ReadOnly]
        public WorkBench.ToolWrapper toolWrapper;

        public WorkBenchSlot(WorkBench.ToolWrapper toolWrapper, IEnumerable<WorkBenchDebuff> debuff)
        {

            this.toolWrapper = toolWrapper;

            previewBuffs = new BehaviorSubject<List<IToolBuff>>(_previewBuffTools);

            buffs = new(_buffTools);
            
            this.debuff.AddRange(debuff);
        }
        
        
        // 是否已经有骰子
        [ShowInInspector, LabelText("是否已经有骰子")]
        public bool isOccupied => pined.Value != null;

        #region tool+buff
        
        private readonly List<IToolBuff> _previewBuffTools = new();
        
        [LabelText("Buff"), ShowInInspector] private readonly List<IToolBuff> _buffTools = new();
        
        [HideInInspector]
        public readonly BehaviorSubject<List<IToolBuff>> buffs;
        [HideInInspector]
        public readonly BehaviorSubject<Tool> preview = new(null);
        [HideInInspector]
        public readonly BehaviorSubject<List<IToolBuff>> previewBuffs;
        [HideInInspector]
        public readonly BehaviorSubject<GameObject> pined = new(null);

        public List<WorkBenchDebuff> debuff = new();

        public void DidForgeThisTurn()
        {
            
            Clear();
        }

        private void Clear()
        {
            if (pined.Value != null && pined.Value.TryGetComponent(out ToolOnTable toolOnTable))
            {
                toolOnTable.DidUsedThisTurn();
            }
            UnPin();
            _buffTools.Clear();
            _previewBuffTools.Clear();
            previewBuffs.OnNext(_previewBuffTools);
            buffs.OnNext(_buffTools);
        }
        
        public void Pin(ToolOnTable toolOnTable)
        {
            pined.OnNext(toolOnTable.gameObject);
        }
        public void UnPin()
        {
            pined.OnNext(null);
        }
        public void PreviewTool(Tool tool)
        {
            preview.OnNext(tool);
        }

        public void NewPreviewBuff(IToolBuff buffTool)
        {
            _previewBuffTools.Add(buffTool);
            previewBuffs.OnNext(_previewBuffTools);
        }

        public void NewBuff(IToolBuff buffTool)
        {
            _buffTools.Add(buffTool);
            buffs.OnNext(_buffTools);
        }
        
        public void ReleaseBuffBy(Guid id)
        {
            var has = _buffTools.Select(v => v.id).Contains(id);
            if (!has) return;
            _buffTools.RemoveAll(v => v.id == id);
            buffs.OnNext(_buffTools);
        }
        
        public void ReleasePreviewBuff()
        {
            _previewBuffTools.Clear();
            previewBuffs.OnNext(_previewBuffTools);
            UnPreviewTool();
        }

        public void UnPreviewTool()
        {
            preview.OnNext(null);
        }
        
        #endregion


        public int CalculateScore()
        {
            if (!isOccupied) return 0;

            var tool = pined.Value.GetComponent<ToolOnTable>().tool;

            var originValue = tool.dice.Roll();

            // 计算buff
            _buffTools.ForEach(v => originValue = v.ValueBy(originValue));
            // 计算debuff
            debuff.ForEach(v => originValue = v.ValueBy(originValue));
            
            return originValue;

        }
        
    }
}