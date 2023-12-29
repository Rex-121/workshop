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
            id = Guid.NewGuid();
        }

        public void LockDice()
        {
            dice = new DicedDice(dice);
        }
        
    }
    
    // public class PinedTool : ITool
    // {
    //     
    //     [ShowInInspector]
    //     public Tool tool;
    //
    //     public PinedTool(Vector2Int position, Tool tool)
    //     {
    //         this.tool = tool;
    //
    //         // buffs = new IToolBuff[] { new LeftSlotBuffTool(position, DicedDice.One(), tool.id), new LeftSlotBuffTool(position, DicedDice.One(), tool.id) };
    //     }
    //
    //     // public readonly IToolBuff[] buffs;
    // }


    // public interface IToolBuff
    // {
    //     
    //     public Guid id { get; }
    //     
    //     public int ValueBy(int value);
    //     
    //     
    //     public Vector2Int effectOnLocation { get; }
    //     
    // }
    //
    // // 左边++buff
    // public struct LeftSlotBuffTool : IToolBuff
    // {
    //     [ShowInInspector]
    //     public Guid id { get; private set; }
    //
    //     public int ValueBy(int value)
    //     {
    //         return value + 1;
    //     }
    //
    //     [ShowInInspector]
    //     public DicedDice dice;
    //
    //     public Vector2Int effectOnLocation { get; private set; }
    //     
    //     public LeftSlotBuffTool(Vector2Int position, DicedDice dice, Guid id)
    //     {
    //         effectOnLocation = position - new Vector2Int(0, 1);
    //         this.dice = dice;
    //         this.id = id;
    //     }
    // }
    
}
