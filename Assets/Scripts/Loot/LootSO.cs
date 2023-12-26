using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    
    [CreateAssetMenu(menuName = "Loot/Loot", fileName = "Loot")]
    public class LootSO: ScriptableObject
    {

        public LootTable<Loot<MaterialSO>, MaterialSO> lootTable;


        [MinMaxSlider(0, 10, true)]
        public Vector2Int lootCount;
        
        public virtual IEnumerable<MaterialSO> GetLoot()
        {

            var count = new RangeInt(lootCount.x, lootCount.y - lootCount.x).RandomInRange();
            
            var array = new MaterialSO[count];
            for (var i = 0; i < count; i++)
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