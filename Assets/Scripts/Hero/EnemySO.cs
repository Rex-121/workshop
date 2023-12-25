using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    [CreateAssetMenu(menuName = "Hero/Enemy", fileName = "Enemy")]
    public class EnemySO: SerializedScriptableObject
    {
        public string enemyName;
        
        [HorizontalGroup("Basic"), HideLabel]
        public EnemyMono enemyMonoPrefab;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public Attribute attribute;
        
        [HideLabel, VerticalGroup("Basic/Att")]
        public HeroHealthStrategy healthStrategy;

        [ShowInInspector]
        public int health => healthStrategy.Health(attribute);


        public LootSO lootSO;

        public ISkill skill;
    }
}