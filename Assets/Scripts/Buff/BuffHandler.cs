using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class BuffHandler
    {
        private readonly LinkedList<BuffInfo> _buffList = new();

        [ShowInInspector]
        public BuffInfo[] buffs => _buffList.ToArray();
        
        public Attack WillHit(Attack attack)
        {
            foreach (var v in _buffList)
            {
                v.buffDataSO.onHit?.Apply(v, attack, newPower => attack = newPower);
            }
            
            return attack;
        }
        
        public Attack WillTakeDamage(Attack attack)
        {
            foreach (var v in _buffList)
            {
                v.buffDataSO.onBeHit?.Apply(v, attack, newPower => attack = newPower);
            }
            
            return attack;
        }
        

        public void UseBuffIfNeeded(Action<BuffInfo> fore)
        {
            _buffList.ForEach(fore.Invoke);
        }

        public void AddBuff(BuffInfo buff)
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
                // 如果buff不存在
                // 加入buff表
                _buffList.AddLast(buff);

                // 按照`priority`进行排序
                InsertionSortLinkedList(_buffList);
                
                foundBuff = buff;
                
            }
            
            // buff 回调 `onCreate`
            foundBuff.buffDataSO.onCreate?.Apply(foundBuff);

        }

        public void RemoveBuff(BuffInfo buff)
        {
            switch (buff.buffDataSO.stackRemoveType)
            {
                case BuffRemoveStackUpdate.Clear:
                    buff.buffDataSO.onRemove?.Apply(buff);
                    _buffList.Remove(buff);
                    break;
                case BuffRemoveStackUpdate.Reduce:
                    buff.currentStack--;

                    buff.buffDataSO.onRemove?.Apply(buff);

                    if (buff.currentStack == 0)
                    {
                        _buffList.Remove(buff);
                    }
                    
                    break;
                default:
                    break;
            }
        }
        

        private BuffInfo FindBuff(int id)
        {
            return _buffList.FirstOrDefault(v => v.buffDataSO.id == id);
        }


        private void InsertionSortLinkedList(LinkedList<BuffInfo> list)
        {
            if (list == null || list.First == null)
            {
                return;
            }

            LinkedListNode<BuffInfo> current = list.First.Next;

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