using System;
using Sirenix.OdinInspector;

namespace Tyrant
{
    [Serializable, HideLabel]
    public struct Attribute: ILiftByQuality<Attribute>
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
        public static Attribute operator +(Attribute a, Attribute b)
        {
            return new Attribute(a.strength + b.strength, a.dexterity + b.dexterity, a.intelligence + b.intelligence);
        }

        public Attribute LiftByQuality(IQuality qualities)
        {
            if (qualities.quality.tier == Quality.Tier.Fine) return this;
            var i = qualities.quality.tier.ToInt();
            return this + new Attribute(i, i, i);
        }
    }

    public enum AttributeTypes
    {
        [LabelText("力量")]
        Strength,
        [LabelText("敏捷")]
        Dexterity,
        [LabelText("智力")]
        Intelligence
    }
    
}