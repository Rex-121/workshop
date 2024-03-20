using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Tyrant
{
    public class CardInfoMono: MonoBehaviour
    {

        public CardEventMessageChannelSO cardEventMessageChannelSO;

        [BoxGroup("UI")]
        public TextMeshProUGUI titleLabel;
        [BoxGroup("UI")]
        public TextMeshProUGUI descriptionLabel;
        
        [ShowInInspector]
        public Tool tool { get; private set; }


        public void NewTool(Tool theTool)
        {
            tool = theTool;

            // 掷骰子
            tool.LockDice();

            titleLabel.text = tool.toolName;
            descriptionLabel.text = tool.description;

        }


        public void Use()
        {
            WorkBenchManager.main.ToolIsSelected(WorkBenchManager.CheckerStatus<Tool>.Leave(tool));
            
            cardEventMessageChannelSO.OnUse(GetComponent<CardPlacementCanvasMono>());

            Destroy(gameObject);
        }
        
    }
}