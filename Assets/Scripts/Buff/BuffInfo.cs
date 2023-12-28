using System;
using UnityEngine;

namespace Tyrant
{
    public class BuffInfo
    {

        public BuffInfo(BuffDataSO s)
        {
            buffDataSO = s;
            currentStack = s.startWithStack;
        }

        public readonly BuffDataSO buffDataSO;

        public int currentStack;

        public string currentStackDisplay => currentStack switch
        {
            > 1 => currentStack.ToString(),
            1 when buffDataSO.startWithStack == 1 => "",
            _ => currentStack.ToString()
        };

        public Sprite icon => buffDataSO.icon;


        public void BattleDidEnd()
        {
            currentStack = buffDataSO.startWithStack;
        }

        /// <summary>
        /// 增加buff层数（如果可以/需要增加）
        /// </summary>
        /// <param name="buffInfo">buff</param>
        public void AddBuffStackIfNeeded(BuffInfo buffInfo)
        {
            switch (buffDataSO.updateTimeType)
            {
                case BuffUpdateTime.Add:
                    if (currentStack < buffDataSO.maxStack)
                    {
                        currentStack = Math.Min(buffDataSO.maxStack, currentStack + 1);
                    }
                    break;
                case BuffUpdateTime.Replace:
                    currentStack = 1;
                    break;
                case BuffUpdateTime.Keep:
                    break;
            }
        }
    }
}