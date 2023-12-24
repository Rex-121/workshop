using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tyrant
{
    
    [CreateAssetMenu(menuName = "Loot/Loot", fileName = "Loot")]
    public class LootSO: ScriptableObject
    {

        public LootTable<Loot<MaterialSO>, MaterialSO> lootTable;


        public int lootCount;
        
        public virtual IEnumerable<MaterialSO> GetLoot()
        {
            var array = new MaterialSO[lootCount];
            for (var i = 0; i < lootCount; i++)
            {
                array[i] = lootTable.GetLoot().loot;
            }
            return array;
        }
        
        
        public virtual void ComputeWeights()
        {
            lootTable.ComputeWeights();
        }
		
        protected virtual void OnValidate()
        {
            ComputeWeights();
        }
        
    }
}