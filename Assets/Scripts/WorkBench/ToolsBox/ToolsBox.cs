using System;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Dicing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant.UI
{
    public class ToolsBox: MonoBehaviour, IWorkBenchRound
    {
        
        /*
         * 1. 初始化牌组
         * 2. 发初始卡牌
         * 3. 每回合发牌
         * 4. 使用后进入弃牌堆
         * 5. 卡牌用尽
         */

        [ShowInInspector, NonSerialized] public Stack<Tool> toolsStack;

        
        public int amountPerTurn = 5;

        public ToolsBoxInCanvas toolsBoxInCanvas;


        [NonSerialized] public CardDeck cardDeck;

        private void Awake()
        {
            cardDeck = new CardDeck();
        }

        // 准备新的一轮铸造
        public void PrepareNewRound()
        {
            ResetBox();
            
           
        }

        public void NewTurn()
        {
           BeforeDrawTools();
        }

        private void ResetBox()
        {
            DuringDrawTools(cardDeck.GenesisDraw());
        }

        public void BeforeDrawTools()
        {
            var drawPerTurn = Protagonist.main.drawPerTurn;
            // 找到需要发送几个工具
            var array = new Tool[drawPerTurn];
            
            (0, drawPerTurn)
                .Enumerate(i => array[i] = cardDeck.Draw());
            
            DuringDrawTools(array);
        }

        public void DidEndRound()
        {
            
        }

        public void DuringDrawTools(Tool[] tools)
        {
            for (var i = 0; i < tools.Count(); i++)
            {
                toolsBoxInCanvas.NewTool(i, tools.ElementAt(i));
            }
        }

        public void EndDrawTools()
        {
            
        }
        
    }
}