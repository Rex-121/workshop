using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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

        public DiceBuffHandler(string style)
        {
            this.style = style;
        }
        // public Attack WillHit(Attack attack)
        // {
        //     foreach (var v in _buffList)
        //     {
        //         v.buffDataSO.onHit?.Apply(v, attack, newPower => attack = newPower);
        //     }
        //     
        //     return attack;
        // }
        //
        // public Attack WillTakeDamage(Attack attack)
        // {
        //     foreach (var v in _buffList)
        //     {
        //         v.buffDataSO.onBeHit?.Apply(v, attack, newPower => attack = newPower);
        //     }
        //     
        //     return attack;
        // }
        //

        // public void UseBuffIfNeeded(Action<BuffInfo> fore)
        // {
        //     _buffList.ForEach(fore.Invoke);
        // }

        public int AllEffect(int startValue)
        {
            var b = startValue;
            var a = buffs
                .Where(v => v.buffDataSO.onPreview != null)
                .Sum(diceBuffInfo => b = diceBuffInfo.buffDataSO.onPreview.Apply(b, diceBuffInfo));
            return b;
        }

        public void AddBuff(DiceBuffInfo buff)
        {
            var foundBuff = FindBuff(buff.buffDataSO.id);

            // 如果buff存在
            if (foundBuff != null)
            {
                // buff叠层
                foundBuff.AddBuffStackIfNeeded(buff);
                
            }
            else
            {
                // 机制原因，这里要拷贝新的对象
                var newBuff = buff.buffDataSO.ToBuff();
                
                // 如果buff不存在
                // 加入buff表
                _buffList.AddLast(newBuff);

                // 按照`priority`进行排序
                InsertionSortLinkedList(_buffList);
                
                foundBuff = newBuff;
                
            }
            
            
            Debug.Log($"#WorkBenchSlot#{style} 增加 Buff `{foundBuff.buffName}`{foundBuff.currentStack} 剩余buff{_buffList.Count()}");

            
            // buff 回调 `onCreate`
            foundBuff.buffDataSO.onCreate?.Apply();

        }

        public void RemoveBuff(DiceBuffInfo buff)
        {

            buff = FindBuff(buff.buffDataSO.id);
            
            if (ReferenceEquals(buff, null)) return;
            
            switch (buff.buffDataSO.stackRemoveType)
            {
                case BuffRemoveStackUpdate.Clear:
                    buff.buffDataSO.onRemove?.Apply();
                    var success = _buffList.Remove(buff);
                    Debug.Log($"#WorkBenchSlot#{style} 移除 Buff `{buff.buffName}{success}`");
                    break;
                case BuffRemoveStackUpdate.Reduce:
                    buff.currentStack--;

                    buff.buffDataSO.onRemove?.Apply();
                    
                    Debug.Log($"#WorkBenchSlot#{style} 移除 Buff `{buff.buffName}`{buff.currentStack}");

                    if (buff.currentStack == 0)
                    {
                        _buffList.Remove(buff);
                        Debug.Log($"#WorkBenchSlot#{style} 强制移除 Buff `{buff.buffName}`");
                    }
                    
                    break;
                default:
                    break;
            }
            
            Debug.Log($"#WorkBenchSlot#{style} 移除 Buff `{buff.buffName}`{buff.currentStack} 剩余buff{_buffList.Count()}");

        }
        

        private DiceBuffInfo FindBuff(int id)
        {
            return _buffList.FirstOrDefault(v => v.buffDataSO.id == id);
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