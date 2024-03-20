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
            return 22;
            return value + buffInfo.diceFace;
        }
        
    }

}