
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tyrant
{
    public class Dungeon
    {

        public string dungeonName => so.dungeonName;

        public Guid id => Guid.NewGuid();

        public readonly Queue<IDungeonNode> trips;

        public DungeonSO so;

        public Dungeon(DungeonSO so)
        {
            this.so = so;
            trips = new Queue<IDungeonNode>(so.trips);
        }
        
        

        private IDungeonNode Next()
        {
            return trips.Count == 0 ? null : trips.Peek();
        }

        public IDungeonNode StartANewNode(HeroSquad heroSquad)
        {
            var node = Next();
            if (node == null) return null;
            so.onNodeAction?.OnStart(this, node, heroSquad);
            node.OnStart(this, node, heroSquad);
            return node;
        }
        
        // 开始`dungeon`
        public void DidStartDungeon(HeroSquad heroSquad)
        {
            so.onDungeonBegin?.Apply(this, heroSquad);
        }

        // 此节点已完成
        public void DidFinishThisNode(HeroSquad heroSquad)
        {
            if (trips.Count == 0) return;
            var node = trips.Dequeue();
            so.onNodeAction?.OnFinish(this, node, heroSquad);
            node.OnFinish(this, node, heroSquad);
        }
        
    }
}