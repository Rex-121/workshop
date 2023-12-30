using Dicing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Tool/Tool", fileName = "_Tool")]
    public class ToolSO: SerializedScriptableObject
    {

        public string toolName;
        
        public int diceFace;

        public string toolDescription;

        public DiceBuffDataSO diceBuffDataSO;
        
        public Tool ToTool() => new Tool(this);
    }
}