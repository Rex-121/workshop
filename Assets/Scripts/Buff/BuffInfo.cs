using System;
using UnityEngine;

namespace Tyrant
{
    public class BuffInfo
    {

        public BuffInfo(BuffDataSO s)
        {
            buffDataSO = s;
        }

        public readonly BuffDataSO buffDataSO;

        public int currentStack = 1;

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