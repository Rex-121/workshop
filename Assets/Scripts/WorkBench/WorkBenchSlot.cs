
using System;
using Sirenix.OdinInspector;
using Tyrant.UI;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class WorkBenchSlot
    {
        
        public string name => $"{toolWrapper.type}-{toolWrapper.position}棋盘格";
        
        [HideLabel, ReadOnly]
        public WorkBench.ToolWrapper toolWrapper;

        public WorkBenchSlot(WorkBench.ToolWrapper toolWrapper)
        {
            this.toolWrapper = toolWrapper;
        }
        
        
        
        
        
        
        
        
        
        

        public MaterialFeatureSO materialFeature;


        public DiceBuffHandler buffHandler = new DiceBuffHandler("实体");
        public DiceBuffHandler previewBuffHandler = new DiceBuffHandler("预览");

        public string monoName => $"{toolWrapper.position}-{materialFeature?.featureName}";
        
        public int AllEffect(int startValue)
        {
            if (ReferenceEquals(materialFeature, null))
            {
                return previewBuffHandler.AllEffect(buffHandler.AllEffect(startValue));
            }
            
            var value = materialFeature.pinDice?.ApplyDice(startValue) ?? new Tuple<bool, int>(true, startValue);

            return value.Item1 ? previewBuffHandler.AllEffect(buffHandler.AllEffect(value.Item2)) : value.Item2;
        }
        
        // 是否已经有骰子
        [ShowInInspector, LabelText("是否已经有骰子")]
        public bool isOccupied => pined.Value != null;


        [ShowInInspector]
        public int diceFace => !isOccupied ? 0 : buffHandler.AllEffect(pined.Value.GetComponent<ToolOnTable>().tool.dice.Roll());

        #region tool+buff
        
        [HideInInspector]
        public readonly BehaviorSubject<ToolOnTable> preview = new(null);
        
        [HideInInspector]
        public readonly BehaviorSubject<ToolOnTable> pined = new(null);
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
            
            buffHandler.Clear();
            previewBuffHandler.Clear();
            
            UnPin();
        }
        
        public void Pin(ToolOnTable toolOnTable)
        {
            toolOnTable.toolWrapper = toolWrapper;
            
            pined.OnNext(toolOnTable);
            
            // 骰子面值发生变化，需要更新buff
            DiceValueDidBuffed();
            
        }
        public void UnPin()
        {
            pined.OnNext(null);
            DiceValueDidBuffed();
        }

        public void PreviewTool(ToolOnTable tool)
        {
            preview.OnNext(tool);

            DiceValueDidBuffed();
        }

        // 骰子面值发生变化，需要更新buff
        private void DiceValueDidBuffed()
        {
            if (pined.Value != null)
            {
                var dice = pined.Value.tool.dice;
                var value = AllEffect(dice.Roll());
                pined.Value.diceBuffInfo.diceFace = value;
            }
            else
            {
                if (preview.Value != null)
                {
                    var dice = preview.Value.tool.dice;
                    var value = AllEffect(dice.Roll());
                    preview.Value.diceBuffInfo.diceFace = value;
                }
            }
            
        }
        
        
        public void NewBuff(DiceBuffInfo buffInfo)
        {
            
            // 是否被特性改造buff
            if (materialFeature != null)
            {
                var buff = materialFeature.buffConfig?.ApplyDice(0) ?? new Tuple<bool, int>(true, 0);
                if (!buff.Item1)
                {
                    return;
                }
            }
            
            previewBuffHandler.RemoveBuff(buffInfo);
            buffHandler.AddBuff(buffInfo);
            
            DiceValueDidBuffed();
        }

        public void NewPreviewBuff(DiceBuffInfo buffInfo)
        {
            
            // 是否被特性改造buff
            if (materialFeature != null)
            {
                var buff = materialFeature.buffConfig?.ApplyDice(0) ?? new Tuple<bool, int>(true, 0);
                if (!buff.Item1)
                {
                    return;
                }
            }

            
            previewBuffHandler.AddBuff(buffInfo);
            DiceValueDidBuffed();
        }
        
        public void RemovePreviewBuff(DiceBuffInfo buffInfo)
        {
            previewBuffHandler.RemoveBuff(buffInfo);
            DiceValueDidBuffed();
        }
        
        public void RemoveBuff(DiceBuffInfo buffInfo)
        {
            buffHandler.RemoveBuff(buffInfo);
            
            DiceValueDidBuffed();
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