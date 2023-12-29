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
    }

    public struct D : IDiceBuffModel
    {
        public void Apply()
        {
        }
    }

    public struct X : IDiceBuffMathModel
    {
        public int Apply(int value, DiceBuffInfo buffInfo)
        {
            return value + 1 * buffInfo.currentStack;
        }
    }

}