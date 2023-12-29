using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class DiceBuffInfo
    {

        public DiceBuffInfo(DiceBuffDataSO s)
        {
            buffDataSO = s;
            currentStack = s.startWithStack;
        }

        public string buffName => buffDataSO.buffName;
        
        public readonly DiceBuffDataSO buffDataSO;

        public int currentStack;

        public int diceFace;

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

        // [ShowInInspector]
        // public IDiceBuffMathModel j => buffDataSO.onDiceFaceChanged;

        /// <summary>
        /// 增加buff层数（如果可以/需要增加）
        /// </summary>
        /// <param name="buffInfo">buff</param>
        public void AddBuffStackIfNeeded(DiceBuffInfo buffInfo)
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