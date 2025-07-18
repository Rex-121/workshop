using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{


    public interface IDungeonNode: IDungeonNodeAction
    {
        public int delay { get; }


        public string icon { get; }
    }
    
    [System.Serializable]
    public struct DungeonTripNode: IDungeonNode
    {

        public EnemySO enemySO;
        
        [SerializeField]
        public int delay { get; set; }

        public string icon => "b";

        public void OnStart(Dungeon dungeon, IDungeonNode node, HeroSquadMono heroSquadMono)
        {
        }

        public void OnFinish(Dungeon dungeon, IDungeonNode node, HeroSquadMono heroSquadMono)
        {
        }
    }


    [LabelText("集体补血")]
    public struct DungeonHealthSupplyTripNode : IDungeonNode
    {

        [LabelText("补充血量")]
        public int healthSupply;

        [SerializeField]
        public int delay { get; set; }
        
        public string icon => "s";
        
        public void OnStart(Dungeon dungeon, IDungeonNode node, HeroSquadMono heroSquadMono)
        {
            var value = healthSupply;
            heroSquadMono.heroes.Select(v => v.heroic).ForEach(v => v.health.TakeDamage(- value));
        }

        public void OnFinish(Dungeon dungeon, IDungeonNode node, HeroSquadMono heroSquadMono)
        {
            
        }
    }
}