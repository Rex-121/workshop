using System;
using Dicing;
using Sirenix.OdinInspector;

namespace Tools
{
    public interface ITool
    {
        
    }


    [Serializable]
    public class Tool : ITool
    {
        
        [ShowInInspector]
        public IDicing dice;

        public Tool(IDicing dice)
        {
            this.dice = dice;
        }

        public void LockDice()
        {
            dice = new DicedDice(dice);
        }
    }
}
