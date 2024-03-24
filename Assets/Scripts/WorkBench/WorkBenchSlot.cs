using System;
using Sirenix.OdinInspector;
using UniRx;

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

            RecalculateSlot();
            
            return true;
        }
        

        /// <summary>
        /// 重新计算该棋盘格的分数
        /// </summary>
        private void RecalculateSlot()
        {
            // 如果没有骰子, 计分为0
            if (tool == null)
            {
                scoreOnSlot.Value = 0;
            }
            else
            {
                scoreOnSlot.Value = buffHandler.AllEffect(tool.dice.Roll());
                // 有的buff依赖当前骰子值
                tool.diceBuffInfo.diceFace = scoreOnSlot.Value;
            }
        }

        public void Recalculate()
        {
            // 重新刷新预览的buff,因为预览buff计算需要实体buff的支持
            previewBuffHandler.Recalculate();
            RecalculateSlot();
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
        [ShowInInspector, LabelText("是否已经有骰子")]
        public bool isOccupied => tool != null;
        
        #region tool+buff
        
        public void DidForgeThisTurn()
        {
            _toolInSlot.Value = null;
            Clear();
            Recalculate();
        }

        private void Clear()
        {
            buffHandler.Clear();
            previewBuffHandler.Clear();
        }

        #endregion


        /// <summary>
        /// 计算分数
        /// </summary>
        /// <returns>分数</returns>
        public int CalculateScore()
        {
            if (!isOccupied) return 0;
        
            // var tool = pined.Value.GetComponent<ToolOnTable>().tool;
        
            var originValue = tool.dice.Roll();
        
            return AllEffect(originValue);
        
        }
        
    }
}