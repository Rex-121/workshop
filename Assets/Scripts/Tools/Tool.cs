using System;
using Dicing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools
{
    public interface ITool
    {
        
    }


    [Serializable]
    public class Tool : ITool
    {
        public Guid id;
        
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
    
    public class PinedTool : ITool
    {

        public Guid id => tool.id;
        
        [ShowInInspector]
        public Tool tool;

        public PinedTool(Vector2Int position, Tool tool)
        {
            this.tool = tool;
            buffs = new[] {new ToolBuff(position, id)};
        }

        public ToolBuff[] buffs;
    }

    public struct SimpleBuffTool : ITool
    {
        public Guid toolId;
        
        [ShowInInspector]
        public DicedDice dice;

        public Vector2Int position;

        public SimpleBuffTool(Vector2Int position, DicedDice dice, Guid id)
        {
            this.position = position;
            this.dice = dice;
            this.toolId = id;
        }
    }

    public struct ToolBuff
    {
        
        public Guid toolId;
        
        public Vector2Int position;

        public ToolBuff(Vector2Int position, Guid toolId)
        {
            this.position = position;
            this.toolId = toolId;
        }
        public SimpleBuffTool simpleBuffTool
        {
            get
            {
                Debug.Log(position);
                Debug.Log(position - new Vector2Int(0, 1));
                return new SimpleBuffTool(position - new Vector2Int(0, 1), DicedDice.One(), toolId);
            }
        }
    }
}
