using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Hero/Hero", fileName = "Hero")]
    public class JobSO: SerializedScriptableObject
    {
        public string jobName;
        
        // [HorizontalGroup("Basic"), HideLabel]
        // public HeroMono heroMonoPrefab;
        
        // [HideLabel, VerticalGroup("Basic/Att")]
        // public Attribute attribute;

        [PreviewField]
        public Sprite icon;

        public AttributeTypes mainAttribute;
        
        // [HideLabel, VerticalGroup("Basic/Att")]
        // public HeroHealthStrategy healthStrategy;

        // [ShowInInspector]
        // public int health => healthStrategy.Health(attribute);

        public BuffDataSO[] skills;


        public WeaponSO weaponSO;
        
        public int AttributePower(Attribute attribute) => mainAttribute switch
        {
            AttributeTypes.Strength => Math.Max(0, (attribute.strength - 2) / 2) + 1,
            AttributeTypes.Dexterity => Math.Max(0, (attribute.dexterity - 2) / 2) + 1,
            AttributeTypes.Intelligence => Math.Max(0, (attribute.intelligence - 2) / 2) + 1,
            _ => 0
        };
    }
}