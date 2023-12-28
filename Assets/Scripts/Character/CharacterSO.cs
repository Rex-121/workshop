using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Hero/Character", fileName = "Character")]
    public class CharacterSO: SerializedScriptableObject
    {
        public string characterName;
        
        [HorizontalGroup("Basic"), HideLabel]
        public CharacterMono characterMonoPrefab;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public Attribute attribute;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public HeroHealthStrategy healthStrategy;

        [ShowInInspector]
        public int health => healthStrategy.Health(attribute);
    }
}