using System;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Dicing
{
    public interface IDicing
    {

        [ShowInInspector]
        // 面数
        public int faces { get; }

        // 投骰子
        public int Roll();

    }



    [Serializable]
    public readonly struct Dice: IDicing
    {
        
        public int faces { get; }

        public Dice(int faces)
        {
            this.faces = faces;
        }

        public int Roll() => Random.Range(1, faces + 1);

        public IDicing diced => new DicedDice(this);
        
        public static IDicing Diced(int faces = 6) => new DicedDice(new Dice(faces));
    }
    
    [Serializable]
    public readonly struct DicedDice: IDicing
    {

        public readonly IDicing dice;
        public int faces => dice.faces;
        
        [ShowInInspector]
        public readonly int value;
        
        public DicedDice(IDicing dice)
        {
            this.dice = dice;
            value = this.dice.Roll();
        }

        public int Roll() => value;
    }
}
