using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Object = System.Object;

namespace Tyrant
{

    public interface IDiceBuffCalculateStrategy
    {
        public int BeforeCalculate();
    }

    internal struct DefaultDiceBuffCalculateStrategy: IDiceBuffCalculateStrategy
    {
        public int BeforeCalculate() => 0;
    }

    /// <summary>
    /// 先计算其他buff处理器的分数，例如实体buff
    /// 计算是以0为基础，不加入骰子的值
    /// </summary>
    public readonly struct CalBuffFirstDiceBuffCalculateStrategy : IDiceBuffCalculateStrategy
    {
        private readonly DiceBuffHandler _handler;

        public CalBuffFirstDiceBuffCalculateStrategy(DiceBuffHandler handler)
        {
            _handler = handler;
        }
        public int BeforeCalculate() => _handler.AllEffect(0);
    }
    
    public class DiceBuffHandler
    {
        private readonly LinkedList<DiceBuffInfo> _buffList = new();

        [ShowInInspector]
        public DiceBuffInfo[] buffs => _buffList.ToArray();

        public string style;

        public WorkBench.ToolWrapper toolWrapper;

        private readonly IDiceBuffCalculateStrategy _strategy;

        public DiceBuffHandler(string style, WorkBench.ToolWrapper toolWrapper, IDiceBuffCalculateStrategy strategy)
        {
            _strategy = strategy ?? new DefaultDiceBuffCalculateStrategy();
            this.style = style;
            this.toolWrapper = toolWrapper;
        }

        public void Clear()
        {
            _buffList.Clear();
            
            Recalculate();
        }

        public int AllEffect(int startValue)
        {
            var b = startValue;
            buffs
                .Where(v => v.buffDataSO.onUse != null)
                .ForEach(diceBuffInfo => b = diceBuffInfo.buffDataSO.onUse.Apply(b, diceBuffInfo));
            return b;
        }
        

        public void AddBuff(DiceBuffInfo buff)
        {
            var foundBuff = FindBuff(buff.id);

            // 如果buff存在
            if (foundBuff != null)
            {
                // buff叠层
                // foundBuff.AddBuffStackIfNeeded(buff);
                
            }
            else
            {
                
                // 机制原因，这里要拷贝新的对象
                var newBuff = buff;//buff.buffDataSO.ToBuff();
                
                // 如果buff不存在
                // 加入buff表
                _buffList.AddLast(newBuff);

                // 按照`priority`进行排序
                InsertionSortLinkedList(_buffList);
                
                foundBuff = newBuff;
                
            }

            if (style == "实体")
            {
                // buff 回调 `onCreate`
                foundBuff.buffDataSO.onPin?.Apply(0, foundBuff);
            }

            
            Debug.Log($"#DICE_BUFF({style})# {toolWrapper.debugDescription} 增加{foundBuff.id} - 当前buff数量{_buffList.Count}");
            
            Recalculate();
        }

        public ReactiveProperty<int> previewScore = new (0);

        public void Recalculate()
        {
            previewScore.Value = AllEffect(_strategy.BeforeCalculate());
        }

        public void RemoveBuff(DiceBuffInfo buff)
        {

            buff = FindBuff(buff.id);
            
            if (ReferenceEquals(buff, null)) return;
            
            buff.buffDataSO.onRemove?.Apply();
            var success = _buffList.Remove(buff);
            
            Debug.Log($"#DICE_BUFF({style})# {toolWrapper.debugDescription} {style}移除{buff.id} - 当前buff数量{_buffList.Count}");

            Recalculate();
        }
        

        private DiceBuffInfo FindBuff(Guid id)
        {
            return _buffList.FirstOrDefault(v => v.id == id);
        }


        private void InsertionSortLinkedList(LinkedList<DiceBuffInfo> list)
        {
            if (list == null || list.First == null)
            {
                return;
            }

            LinkedListNode<DiceBuffInfo> current = list.First.Next;

            while (current != null)
            {
                var next = current.Next;
                var prev = current.Previous;
                while (prev != null && prev.Value.buffDataSO.priority > current.Value.buffDataSO.priority)
                {
                    prev = prev.Previous;
                }

                if (prev == null)
                {
                    // current成为头节点
                    list.Remove(current);
                    list.AddFirst(current);
                }
                else
                {
                    // current插入prev之前
                    list.Remove(current);
                    list.AddAfter(prev, current);
                }

                current = next;
            }
        }
    }
}