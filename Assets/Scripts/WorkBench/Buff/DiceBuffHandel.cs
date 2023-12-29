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

        public int AllEffect(int startValue)
        {
            var b = startValue;
            buffs
                .Where(v => v.buffDataSO.onUse != null)
                .ForEach(diceBuffInfo => b = diceBuffInfo.buffDataSO.onUse.Apply(b, diceBuffInfo));
            
            
            
            
            return b;
        }

        // public int AllEffectByFace(int face)
        // {
        //     var b = face;
        //     buffs
        //         .Where(v => v.buffDataSO.onDiceFaceChanged != null)
        //         .ForEach(diceBuffInfo => b = diceBuffInfo.buffDataSO.onDiceFaceChanged.Apply(b, diceBuffInfo));
        //     
        //     return b;
        // }

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
                    break;
                case BuffRemoveStackUpdate.Reduce:
                    buff.currentStack--;

                    buff.buffDataSO.onRemove?.Apply();
                    

                    if (buff.currentStack == 0)
                    {
                        _buffList.Remove(buff);
                    }
                    
                    break;
                default:
                    break;
            }
            
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