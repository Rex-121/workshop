using System;
using Sirenix.OdinInspector;

namespace Tyrant
{
    
    [Serializable, HideLabel]
    public struct HeroHealthStrategy
    {
        [HorizontalGroup("Att"), BoxGroup("Att/力量倍数"), HideLabel]
        public int strength;
        
        [HorizontalGroup("Att"), BoxGroup("Att/敏捷倍数"), HideLabel]
        public int dexterity;

        [HorizontalGroup("Att"), BoxGroup("Att/智力倍数"), HideLabel]
        public int intelligence;

        [LabelText("基础生命")]
        public int baseHealth;


        public HeroHealthStrategy(int s, int d, int i, int b)
        {
            strength = s;
            dexterity = d;
            intelligence = i;
            baseHealth = b;
        }
        
        public int Health(Attribute attribute)
        {
            return attribute.strength * strength
                   + attribute.dexterity * dexterity
                   + attribute.intelligence * intelligence
                   + baseHealth;
        }
    }
    
    
}