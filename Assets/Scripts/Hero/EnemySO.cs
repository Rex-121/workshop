using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    [CreateAssetMenu(menuName = "Hero/Enemy", fileName = "Enemy")]
    public class EnemySO: SerializedScriptableObject
    {
        public string enemyName;
        
        public CharacterSO characterSO;
        
        public LootSO lootSO;
        
        public BuffDataSO[] skills;
    }
}