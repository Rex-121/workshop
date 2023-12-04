using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dicing
{
    [CreateAssetMenu(fileName = "Dice样式")]
    public class DiceSpriteDefineSO: SerializedScriptableObject
    {
        
        [ShowInInspector]
        public Dictionary<int, Sprite> sprites;


        
        // public Sprite dice1;
        // public Sprite dice2;
        // public Sprite dice3;
        // public Sprite dice4;
        // public Sprite dice5;
        // public Sprite dice6;


    }
}