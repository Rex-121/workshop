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
    public class DiceBuffHandler
    {
        private readonly LinkedList<DiceBuffInfo> _buffList = new();

        [ShowInInspector]
        public DiceBuffInfo[] buffs => _buffList.ToArray();

        public string style;

        public WorkBench.ToolWrapper toolWrapper;

        public DiceBuffHandler(string style, WorkBench.ToolWrapper toolWrapper)
        {
            this.style = style;
            this.toolWrapper = toolWrapper;
        }

        public void Clear()
        {
            _buffList.Clear();
        }

        public int AllEffect(int startValue)
        {
            var b = startValue;
            buffs
                .Where(v => v.buffDataSO.onUse != null)
                .ForEach(diceBuffInfo => b = diceBuffInfo.buffDataSO.onUse.Apply(b, diceBuffInfo));
            Debug.Log($"{toolWrapper.position} - - {b}");
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
            
            // buff 回调 `onCreate`
            foundBuff.buffDataSO.onCreate?.Apply();
            
            Debug.Log($"Add Buff {_buffList.Count}");
        }

        public ReactiveProperty<int> previewScore = new ReactiveProperty<int>(0);
        public void PreviewBuff(int startValue)
        {
            var value = AllEffect(startValue);
            Debug.Log($"{startValue}-{value}");
            previewScore.Value = value;
        }

        public void RemoveBuff(DiceBuffInfo buff)
        {

            buff = FindBuff(buff.id);
            
            if (ReferenceEquals(buff, null)) return;
            
            buff.buffDataSO.onRemove?.Apply();
            var success = _buffList.Remove(buff);
            Debug.Log($"#Buff# remove {success}");
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