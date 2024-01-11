
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Hero/Character", fileName = "Character")]
    public class CharacterSO: SerializedScriptableObject
    {
        public int id;
        
        public string characterName;
        
        [HorizontalGroup("Basic"), VerticalGroup("Basic/Basic"), HideLabel]
        public CharacterMono characterMonoPrefab;


        [HorizontalGroup("Basic"),VerticalGroup("Basic/Basic"), HideLabel, PreviewField(60), ShowInInspector]
        public Sprite avatar => sprites.FirstOrDefault();
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public Attribute attribute;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public HeroHealthStrategy healthStrategy;

        [ShowInInspector]
        public int health => healthStrategy.Health(attribute);


        public Sprite[] sprites;
        
    }
}