using System;
using Sirenix.OdinInspector;

namespace Tyrant
{
    [Serializable, HideLabel]
    public struct Attribute
    {
        [HorizontalGroup("Att"), BoxGroup("Att/力量"), HideLabel]
        public int strength;
        
        [HorizontalGroup("Att"), BoxGroup("Att/敏捷"), HideLabel]
        public int dexterity;

        [HorizontalGroup("Att"), BoxGroup("Att/智力"), HideLabel]
        public int intelligence;

        public Attribute(int strength, int dexterity, int intelligence)
        {
            this.strength = strength;
            this.dexterity = dexterity;
            this.intelligence = intelligence;
        }

        public Attribute LiftByQuality(IQuality quality)
        {
            var add = quality.quality.tier.ToInt();
            return new Attribute(strength + add, dexterity + add, intelligence + add);
        }


        public static Attribute operator +(Attribute a, Attribute b)
        {
            return new Attribute(a.strength + b.strength, a.dexterity + b.dexterity, a.intelligence + b.intelligence);
        }
    }
    
    
}