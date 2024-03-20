
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
            
           buffHandler = new DiceBuffHandler("实体", toolWrapper, null);
           previewBuffHandler = new DiceBuffHandler("预览", toolWrapper, new CalBuffFirstDiceBuffCalculateStrategy(buffHandler));
        }

        public Tool tool => _toolInSlot.Value;
        public IObservable<Tool> toolInSlot => _toolInSlot;

        public ReactiveProperty<int> scoreOnSlot = new ReactiveProperty<int>(0);

        // public IObservable<WorkBenchSlot> toolDidAddInSlot => toolInSlot.Select(v => this);
        
        /// <summary>
        /// 选中的手牌
        /// </summary>
        private readonly ReactiveProperty<Tool> _toolInSlot = new ReactiveProperty<Tool>(null);

        /// <summary>
        /// 放置新的工具
        /// </summary>
        /// <param name="toolOnSlot">toolOnSlot</param>
        public bool PlaceToolInSlot(Tool toolOnSlot)
        {
            if (tool != null) return false;
            
            _toolInSlot.Value = toolOnSlot;

            RecalculateDice();
            
            return true;
        }
        

        private void RecalculateDice()
        {
            if (tool == null) return;
            scoreOnSlot.Value = buffHandler.AllEffect(tool.dice.Roll());
            // 有的buff依赖当前骰子值
            tool.diceBuffInfo.diceFace = scoreOnSlot.Value;
        }

        public void Recalculate()
        {
            // 重新刷新预览的buff,因为预览buff计算需要实体buff的支持
            previewBuffHandler.Recalculate();
            RecalculateDice();
        }


        /// <summary>
        /// 添加buff
        /// </summary>
        /// <param name="buff">buff</param>
        public void AddBuff(DiceBuffInfo buff)
        {
            buffHandler.AddBuff(buff);
        }

        public void PreviewBuff(bool isAdd, DiceBuffInfo buff)
        {
            if (isAdd)
            {
                previewBuffHandler.AddBuff(buff);
            }
            else
            {
                previewBuffHandler.RemoveBuff(buff);
            }
        }

        // public void UpdatePreviewBuff()
        // {
        //     previewBuffHandler.PreviewBuff(buffHandler.AllEffect(0));
        // }

        public MaterialFeatureSO materialFeature;


        public DiceBuffHandler buffHandler;
        public DiceBuffHandler previewBuffHandler;

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
        // [ShowInInspector, LabelText("是否已经有骰子")]
        // public bool isOccupied => pined.Value != null;


        // [ShowInInspector]
        // public int diceFace => !isOccupied ? 0 : buffHandler.AllEffect(pined.Value.GetComponent<ToolOnTable>().tool.dice.Roll());

        #region tool+buff
        
        // [HideInInspector]
        // public readonly BehaviorSubject<ToolOnTable> preview = new(null);
        //
        // [HideInInspector]
        // public readonly BehaviorSubject<ToolOnTable> pined = new(null);
        public void DidForgeThisTurn()
        {
            Clear();
        }

        private void Clear()
        {
            // if (pined.Value != null && pined.Value.TryGetComponent(out ToolOnTable toolOnTable))
            // {
            //     toolOnTable.DidUsedThisTurn();
            // }
            //
            // buffHandler.Clear();
            // previewBuffHandler.Clear();
            //
            // UnPin();
        }
        
        // public void Pin(ToolOnTable toolOnTable)
        // {
        //     toolOnTable.toolWrapper = toolWrapper;
        //     
        //     pined.OnNext(toolOnTable);
        //     
        //     // 骰子面值发生变化，需要更新buff
        //     DiceValueDidBuffed();
        //     
        // }
        // public void UnPin()
        // {
        //     pined.OnNext(null);
        //     DiceValueDidBuffed();
        // }
        //
        // public void PreviewTool(ToolOnTable tool)
        // {
        //     preview.OnNext(tool);
        //
        //     DiceValueDidBuffed();
        // }
        //
        // // 骰子面值发生变化，需要更新buff
        // private void DiceValueDidBuffed()
        // {
        //     if (pined.Value != null)
        //     {
        //         var dice = pined.Value.tool.dice;
        //         var value = AllEffect(dice.Roll());
        //         pined.Value.diceBuffInfo.diceFace = value;
        //     }
        //     else
        //     {
        //         if (preview.Value != null)
        //         {
        //             var dice = preview.Value.tool.dice;
        //             var value = AllEffect(dice.Roll());
        //             preview.Value.diceBuffInfo.diceFace = value;
        //         }
        //     }
        //     
        // }
        
        
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
            
            // DiceValueDidBuffed();
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
            // DiceValueDidBuffed();
        }
        
        public void RemoveBuff(DiceBuffInfo buffInfo)
        {
            buffHandler.RemoveBuff(buffInfo);
            
            // DiceValueDidBuffed();
        }


        
        // public void UnPreviewTool()
        // {
        //     preview.OnNext(null);
        // }
        
        #endregion


        // public int CalculateScore()
        // {
        //     if (!isOccupied) return 0;
        //
        //     var tool = pined.Value.GetComponent<ToolOnTable>().tool;
        //
        //     var originValue = tool.dice.Roll();
        //
        //     return AllEffect(originValue);
        //
        // }
        
    }
}