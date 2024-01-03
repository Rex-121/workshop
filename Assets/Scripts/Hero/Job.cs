using System;
using UnityEngine;

namespace Tyrant
{
    [Serializable]
    public struct Job
    {
        public int id;
        
        public string jobName;

        public JobSO jobSO => HeroGenesis.main.FindJobByID(id);

        public Sprite icon => jobSO.icon;
        
        public AttributeTypes mainAttribute;

        public Job(int id, string name, Sprite icon, AttributeTypes mainAttribute)
        {
            this.id = id;
            jobName = name;
            this.mainAttribute = mainAttribute;
        }
        
        public int AttributePower(Attribute attribute) => mainAttribute switch
        {
            AttributeTypes.Strength => Math.Max(0, (attribute.strength - 2) / 2) + 1,
            AttributeTypes.Dexterity => Math.Max(0, (attribute.dexterity - 2) / 2) + 1,
            AttributeTypes.Intelligence => Math.Max(0, (attribute.intelligence - 2) / 2) + 1,
            _ => 0
        };
    }
}