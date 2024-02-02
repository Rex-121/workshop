

using System;
using System.Collections;
using System.Collections.Generic;
using Algorithm;
using Sirenix.OdinInspector;

namespace Tyrant
{
    
    
    public class CardDeck
    {
        
        /*
         * 1. 初始化牌组
         * 2. 发初始卡牌
         * 3. 每回合发牌
         * 4. 使用后进入弃牌堆
         * 5. 卡牌用尽
         */
        
        [ShowInInspector] public Stack<Tool> toolsStack;
        
        
        private static IEnumerable<ToolSO> toolSos => Protagonist.main.toolSos;
        
        [ShowInInspector]
        public CardTomb tomb = new CardTomb();
        
        public CardDeck()
        {
            var list = new List<Tool>();
            
            // 初始化牌组
            (0, Protagonist.main.maxCardDeckCapacity)
                .ForEach(() => list.Add(toolSos.RandomElement().ToTool()));
            
            
            toolsStack = new Stack<Tool>(list);
        }



        public Tool[] GenesisDraw()
        {
            var amount = Protagonist.main.genesisCardAmount;
            var array = new Tool[amount];
            (0, amount)
                .Enumerate(v => array[v] = Draw());
            return array;
        }

        // 发牌
        public Tool Draw()
        {
            var has = toolsStack.TryPop(out Tool tool);
            if (!has) return null;
            return tool;
        }

        
        public bool isAllUsed => toolsStack.Count == 0;

        public void Die(Tool tool)
        {
            tomb.Add(tool);
        }
        
        
    }



    [HideReferenceObjectPicker, LabelText("弃牌堆")]
    public class CardTomb: ICollection<Tool>
    {
        private readonly List<Tool> _tools = new List<Tool>();
        
        public IEnumerator<Tool> GetEnumerator() => _tools.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _tools.GetEnumerator();

        public void Add(Tool item) => _tools.Add(item);

        public void Clear() => _tools.Clear();

        public bool Contains(Tool item) => _tools.Contains(item);

        public void CopyTo(Tool[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Tool item) => _tools.Remove(item);

        public int Count => _tools.Count;
        public bool IsReadOnly => true;
    }
}