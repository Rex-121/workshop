using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public interface IDungeonAction
    {

        public void Apply(Dungeon dungeon, HeroSquadMono heroSquadMono);

    }
    
    public interface IDungeonNodeAction
    {
        public void OnStart(Dungeon dungeon, IDungeonNode node, HeroSquadMono heroSquadMono);
        public void OnFinish(Dungeon dungeon, IDungeonNode node, HeroSquadMono heroSquadMono);
    }


    public struct DefaultDungeonBeginAction: IDungeonAction
    {
        public void Apply(Dungeon dungeon, HeroSquadMono heroSquadMono)
        {
            Debug.Log($"#Dungeon# {dungeon.dungeonName}({dungeon.id.ToString().Split("-").First()}) 开始");
        }
    }
}