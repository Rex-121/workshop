using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [System.Serializable]
    public class HeroSquad: IDoCollectItem
    {
        public Hero[] heroes => _heroes.ToArray();

        [NonSerialized]
        public bool isOnAdventure = false;

        public void OnAdventure() => isOnAdventure = true;
        
        [SerializeField]
        private List<Hero> _heroes;


        [ShowInInspector]
        public SquadInventory inventory = new();

        public HeroSquad(IEnumerable<Hero> heroes)
        {
            _heroes = heroes.ToList();
        }

        
        // 收集道具
        public void CollectItem(IItem item)
        {
            inventory?.CollectItem(item);
        }

        // 恢复存档
        public void Restore()
        {
            _heroes.ForEach(v => v.Restore());
        }



        // #region ICollection
        //
        // public IEnumerator<Hero> GetEnumerator() => _heroes.GetEnumerator();
        //
        // IEnumerator IEnumerable.GetEnumerator() => _heroes.GetEnumerator();
        //
        // public void Add(Hero item) => _heroes.Add(item);
        //
        // public void Clear() => _heroes.Clear();
        //
        // public bool Contains(Hero item) => _heroes.Contains(item);
        //
        // public void CopyTo(Hero[] array, int arrayIndex) => _heroes.CopyTo(array, arrayIndex);
        //
        // public bool Remove(Hero item) => _heroes.Remove(item);
        //
        // public int Count => _heroes.Count;
        // public bool IsReadOnly => true;
        //
        // #endregion

        
    }
}