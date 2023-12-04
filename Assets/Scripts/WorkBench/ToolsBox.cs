using System;
using Tools;
using System.Collections.Generic;
using System.Linq;
using Dicing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant.UI
{
    public class ToolsBox: MonoBehaviour
    {
        public static ToolsBox main;

        public Transform dragLayer;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        [ShowInInspector, NonSerialized] public Stack<Tool> toolsStack;

        
        public int amountPerTurn = 5;

        public ToolsBoxInCanvas toolsBoxInCanvas;
        
        private void Start()
        {
            NewTurn();
        }

        public void NewTurn()
        {
           ResetBox();

           BeforeDrawTools();
        }

        private void ResetBox()
        {
            var list = new List<Tool>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Tool(Dice.Diced()));
            }
            toolsStack = new Stack<Tool>(list);
        }

        public void BeforeDrawTools()
        {
            // 找到需要发送几个工具
            var array = new Tool[amountPerTurn];
            for (var i = 0; i < amountPerTurn; i++)
            {
                array[i] = toolsStack.Pop();
            }
            
            DuringDrawTools(array);
        }

        public void DuringDrawTools(IEnumerable<Tool> tools)
        {
            var enumerable = tools as Tool[] ?? tools.ToArray();
            for (var i = 0; i < enumerable.Count(); i++)
            {
                toolsBoxInCanvas.NewTool(i, enumerable.ElementAt(i));
            }
        }

        public void EndDrawTools()
        {
            
        }
        
    }
}