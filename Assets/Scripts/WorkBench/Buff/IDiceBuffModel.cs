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
        public int Apply(int value, DiceBuffInfo buffInfo);

        public void DiceFaceChange(int value);
    }

    public struct D : IDiceBuffModel
    {
        public void Apply()
        {
        }
    }

    // 根据层数增加点数
    [System.Serializable]
    public struct DiceAddSimpleValue : IDiceBuffMathModel
    {

        [LabelText("层数倍率")]
        public int rate;
        
        public int Apply(int value, DiceBuffInfo buffInfo)
        {
            return value + rate * buffInfo.currentStack;
        }

        public void DiceFaceChange(int value)
        {
        }
    }
    
    [System.Serializable]
    public class DiceWithFaceValue : IDiceBuffMathModel
    {

        [ShowInInspector]
        public int v;
        
        public int Apply(int value, DiceBuffInfo buffInfo)
        {
            // Debug.Log($"#DiceWithFaceValue#Apply {value}");
            return v;
        }

        public void DiceFaceChange(int value)
        {
            Debug.Log($"#DiceWithFaceValue#DiceFaceChange {value}");
            v = value;
        }
    }

}