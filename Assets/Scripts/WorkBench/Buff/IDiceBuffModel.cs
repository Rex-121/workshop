using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public interface IDiceBuffModel
    {
        public void Apply();
    }

    
    public interface IDiceBuffMathModel
    {
        // public int Apply(int value, DiceBuffInfo buffInfo, Tool tool, DiceBuffHandler buffHandler);

        public int Apply(int value, DiceBuffInfo buffInfo);
    }

    // 根据层数增加点数
    [System.Serializable]
    public struct DiceAddSimpleValue : IDiceBuffMathModel
    {

        [LabelText("层数倍率")]
        public int rate;

        public int Apply(int value, DiceBuffInfo buffInfo)
        {
            return value + rate;
        }
    }
    
    [System.Serializable]
    public class DiceWithFaceValue : IDiceBuffMathModel
    {

        public int Apply(int value, DiceBuffInfo buffInfo)
        {
            return value + buffInfo.diceFace;
        }
        
    }

    public struct DrawCard : IDiceBuffMathModel
    {
        [LabelText("抽几张牌")]
        public int amount;

        public int Apply(int value, DiceBuffInfo buffInfo)
        {
            WorkBenchManager.main.DrawCardsIfNeeded(amount);
            return 0;
        }
    }

}