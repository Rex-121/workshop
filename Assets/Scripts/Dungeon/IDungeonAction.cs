using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public interface IDungeonAction
    {

        public void Apply(Dungeon dungeon, HeroSquad heroSquad);

    }
    
    public interface IDungeonNodeAction
    {
        public void OnStart(Dungeon dungeon, IDungeonNode node, HeroSquad heroSquad);
        public void OnFinish(Dungeon dungeon, IDungeonNode node, HeroSquad heroSquad);
    }


    public struct DefaultDungeonBeginAction: IDungeonAction
    {
        public void Apply(Dungeon dungeon, HeroSquad heroSquad)
        {
            Debug.Log($"#Dungeon# {dungeon.dungeonName}({dungeon.id.ToString().Split("-").First()}) 开始");
        }
    }
}