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
        
        // public int AllEffectByFace(int startValue)
        // {
        //     return previewBuffHandler.AllEffectByFace(buffHandler.AllEffectByFace(startValue));
        // }
        
        // 是否已经有骰子
        [ShowInInspector, LabelText("是否已经有骰子")]
        public bool isOccupied => pined.Value != null;


        [ShowInInspector]
        public int diceFace
        {
            get
            {
                if (!isOccupied) return 0;
                return buffHandler.AllEffect(pined.Value.GetComponent<ToolOnTable>().tool.dice.Roll());
            }
        }
        
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
        }
        
        public void Pin(ToolOnTable toolOnTable)
        {
            toolOnTable.toolWrapper = toolWrapper;
            
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

        public void NewPreviewBuff(DiceBuffInfo buffInfo)
        {
            previewBuffHandler.AddBuff(buffInfo);

            // if (pined.Value != null)
            // {
            //     var dice = pined.Value.GetComponent<ToolOnTable>().tool.dice;
            //
            //     var value = buffHandler.AllEffect(dice.Roll());
            //
            //     buffHandler.buffs.ForEach(v => v.buffDataSO.onDiceFaceChanged?.DiceFaceChange(value));
            // }
        }
        
        public void RemovePreviewBuff(DiceBuffInfo buffInfo)
        {
            previewBuffHandler.RemoveBuff(buffInfo);
        }
        
        public void RemoveBuff(DiceBuffInfo buffInfo)
        {
            buffHandler.RemoveBuff(buffInfo);
        }

        public void NewBuff(DiceBuffInfo buffInfo)
        {
            previewBuffHandler.RemoveBuff(buffInfo);
            buffHandler.AddBuff(buffInfo);
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

            return AllEffect(originValue);

        }
        
    }
}