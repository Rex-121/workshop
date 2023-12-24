using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [System.Serializable]
    public class LootTable<T, V> where T: Loot<V>
    {
        [SerializeField, ShowInInspector]
        public List<T> ObjectsToLoot;
        
        [ReadOnly]
        public float WeightsTotal;
        protected float _maximumWeightSoFar = 0f;
        protected bool _weightsComputed = false;
        
        public virtual T GetLoot()
        {	
            if (ObjectsToLoot == null)
            {
                return null;
            }

            if (ObjectsToLoot.Count == 0)
            {
                return null;
            }

            if (!_weightsComputed)
            {
                ComputeWeights();
            }
            
            float index = Random.Range(0, WeightsTotal);
 
            foreach (T lootDropItem in ObjectsToLoot)
            {
                if ((index > lootDropItem.RangeFrom) && (index < lootDropItem.RangeTo))
                {
                    return lootDropItem;
                }
            }	
            
            return null;
        }
    
        public virtual void ComputeWeights()
        {
            if (ObjectsToLoot == null)
            {
                return;
            }

            if (ObjectsToLoot.Count == 0)
            {
                return;
            }

            _maximumWeightSoFar = 0f;

            foreach(var lootDropItem in ObjectsToLoot)
            {
                if(lootDropItem.Weight >= 0f)
                {
                    lootDropItem.RangeFrom = _maximumWeightSoFar;
                    _maximumWeightSoFar += lootDropItem.Weight;	
                    lootDropItem.RangeTo = _maximumWeightSoFar;
                } 
                else 
                {
                    lootDropItem.Weight =  0f;						
                }
            }

            WeightsTotal = _maximumWeightSoFar;

            foreach(var lootDropItem in ObjectsToLoot)
            {
                lootDropItem.ChancePercentage = ((lootDropItem.Weight) / WeightsTotal) * 100;
            }

            _weightsComputed = true;
        }
    }
}