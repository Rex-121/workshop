using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Dungeon/Dungeon", fileName = "_Dungeon")]
    public class DungeonSO : SerializedScriptableObject
    {
        
        
        public string dungeonName;

        [SerializeField]
        public IDungeonNode[] trips;
        
        
        public IDungeonNodeAction onNodeAction;

        public IDungeonAction onDungeonBegin;
        public IDungeonAction onDungeonEnd;


        public Dungeon toDungeon => new Dungeon(Instantiate(this));
    }
}
