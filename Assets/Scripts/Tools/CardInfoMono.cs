using System;
using Dicing;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class CardInfoMono: MonoBehaviour
    {

        public CardEventMessageChannelSO cardEventMessageChannelSO;

        [BoxGroup("UI")]
        public TextMeshProUGUI titleLabel;
        [BoxGroup("UI")]
        public TextMeshProUGUI descriptionLabel;


        public DiceSpriteDefineSO diceSpriteDefineSO;
        public Image diceDisplay;
        
        [ShowInInspector]
        public Tool tool { get; private set; }


        public CardPlacementCanvasMono placementCanvasMono;

        private void Awake()
        {
            placementCanvasMono = GetComponent<CardPlacementCanvasMono>();
        }

        public void NewTool(Tool theTool)
        {
            tool = theTool;

            // 掷骰子
            tool.LockDice();

            diceDisplay.sprite = diceSpriteDefineSO.sprites[tool.dice.Roll()];

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