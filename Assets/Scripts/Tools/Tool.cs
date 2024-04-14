using System;
using Dicing;
using Sirenix.OdinInspector;

namespace Tyrant
{
    public interface ITool
    {
        
    }


    [Serializable]
    public class Tool : ITool, WorkBenchManager.ICheckerStatus
    {
        public Guid id = Guid.NewGuid();
        
        [ShowInInspector]
        public IDicing dice;

        public string toolName;
        
        public string description;
        
        public Tool(ToolSO toolSO)
        {
            toolName = toolSO.toolName;
            dice = new Dice(toolSO.diceFace);
            description = toolSO.toolDescription;
            diceBuffInfo = toolSO.diceBuffDataSO.ToBuff();
        }
        
        public DiceBuffInfo diceBuffInfo;
        
        
        // public int Ca

        public void LockDice()
        {
            dice = new DicedDice(dice);

            diceBuffInfo.diceFace = dice.Roll();
        }

        public string debugDescription => $"-卡牌 {toolName}";
    }
    
}
