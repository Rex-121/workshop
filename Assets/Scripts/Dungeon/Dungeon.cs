
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tyrant
{
    public class Dungeon
    {

        public string dungeonName => so.dungeonName;

        public Guid id => Guid.NewGuid();

        public readonly Queue<IDungeonNode> trips;

        public DungeonSO so;

        public string roadMap;

        public Dungeon(DungeonSO so)
        {
            this.so = so;
            trips = new Queue<IDungeonNode>(so.trips);
            roadMap = string.Join("-", trips.Select(v => v.icon));
        }
        
        

        private IDungeonNode Next()
        {
            return trips.Count == 0 ? null : trips.Peek();
        }

        public IDungeonNode StartANewNode(HeroSquadMono heroSquadMono)
        {
            var node = Next();
            if (node == null) return null;
            so.onNodeAction?.OnStart(this, node, heroSquadMono);
            node.OnStart(this, node, heroSquadMono);
            return node;
        }
        
        // 开始`dungeon`
        public void DidStartDungeon(HeroSquadMono heroSquadMono)
        {
            so.onDungeonBegin?.Apply(this, heroSquadMono);
        }

        // 此节点已完成
        public void DidFinishThisNode(HeroSquadMono heroSquadMono)
        {
            if (trips.Count == 0) return;
            
            var node = trips.Dequeue();

            var array = roadMap.Split("-");

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == node.icon)
                {
                    array[i] = "x";
                    break;
                }
            }

            roadMap = string.Join("-", array);
            
            so.onNodeAction?.OnFinish(this, node, heroSquadMono);
            node.OnFinish(this, node, heroSquadMono);
        }
        
    }
}