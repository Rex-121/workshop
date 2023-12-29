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

        public WorkBenchSlot(WorkBench.ToolWrapper toolWrapper)
        {
            this.toolWrapper = toolWrapper;
        }


        public DiceBuffHandler buffHandler = new DiceBuffHandler("实体");
        public DiceBuffHandler previewBuffHandler = new DiceBuffHandler("预览");

        
        public int AllEffect(int startValue)
        {
            return previewBuffHandler.AllEffect(buffHandler.AllEffect(startValue));
        }
        
        // 是否已经有骰子
        [ShowInInspector, LabelText("是否已经有骰子")]
        public bool isOccupied => pined.Value != null;

        #region tool+buff
        
        [HideInInspector]
        public readonly BehaviorSubject<Tool> preview = new(null);
        
        [HideInInspector]
        public readonly BehaviorSubject<GameObject> pined = new(null);
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
            // _buffTools.Clear();
            // _previewBuffTools.Clear();
            // previewBuffs.OnNext(_previewBuffTools);
            // buffs.OnNext(_buffTools);
        }
        
        public void Pin(ToolOnTable toolOnTable)
        {
            toolOnTable.toolWrapper = toolWrapper;
            
            pined.OnNext(toolOnTable.gameObject);
            
            // NewBuff(toolOnTable.diceBuffDataSO.ToBuff());
        }
        public void UnPin()
        {
            pined.OnNext(null);
        }
        public void PreviewTool(Tool tool)
        {
            preview.OnNext(tool);
        }

        public void NewPreviewBuff(DiceBuffInfo buffInfo)
        {
            Debug.Log($"#WorkBenchSlot#NewPreviewBuff {toolWrapper.position} --->");
            previewBuffHandler.AddBuff(buffInfo);
            Debug.Log($"#WorkBenchSlot#NewPreviewBuff {toolWrapper.position} <---\n");
        }
        
        public void RemovePreviewBuff(DiceBuffInfo buffInfo)
        {
            Debug.Log($"#WorkBenchSlot#RemovePreviewBuff {toolWrapper.position} --->");
            previewBuffHandler.RemoveBuff(buffInfo);
            Debug.Log($"#WorkBenchSlot#RemovePreviewBuff {toolWrapper.position} <---\n");
        }
        
        public void RemoveBuff(DiceBuffInfo buffInfo)
        {
            Debug.Log($"#WorkBenchSlot#RemoveBuff {toolWrapper.position} --->");
            buffHandler.RemoveBuff(buffInfo);
            Debug.Log($"#WorkBenchSlot#RemoveBuff {toolWrapper.position} <---\n");
        }

        public void NewBuff(DiceBuffInfo buffInfo)
        {
            // _buffTools.Add(buffTool);
            // buffs.OnNext(_buffTools);
            Debug.Log($"#WorkBenchSlot#NewBuff {toolWrapper.position} --->");
            previewBuffHandler.RemoveBuff(buffInfo);
            buffHandler.AddBuff(buffInfo);
            Debug.Log($"#WorkBenchSlot#NewBuff {toolWrapper.position} <---\n");
        }
        
        // public void ReleasePreviewBuff()
        // {
        //     UnPreviewTool();
        // }
        //
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

            return AllEffect(originValue);
            // 计算buff
            // _buffTools.ForEach(v => originValue = v.ValueBy(originValue));
            // 计算debuff
            // debuff.ForEach(v => originValue = v.ValueBy(originValue));
            
            // return originValue;

        }
        
    }
}