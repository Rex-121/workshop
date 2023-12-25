using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Hero/Hero", fileName = "Hero")]
    public class JobSO: SerializedScriptableObject
    {
        public string jobName;
        
        [HorizontalGroup("Basic"), HideLabel]
        public HeroMono heroMonoPrefab;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public Attribute attribute;

        public AttributeTypes mainAttribute;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public HeroHealthStrategy healthStrategy;

        [ShowInInspector]
        public int health => healthStrategy.Health(attribute);

        public ISkill[] skills;
    }
}