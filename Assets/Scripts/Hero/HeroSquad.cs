using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tyrant
{
    [System.Serializable]
    public class HeroSquad: ICollection<Hero>
    {
        
        
        [SerializeField]
        private List<Hero> _heroes;


        public HeroSquad(IEnumerable<Hero> heroes)
        {
            _heroes = heroes.ToList();
        }


        public void Restore()
        {
            _heroes.ForEach(v => v.Restore());
        }



        #region ICollection

        public IEnumerator<Hero> GetEnumerator() => _heroes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _heroes.GetEnumerator();

        public void Add(Hero item) => _heroes.Add(item);

        public void Clear() => _heroes.Clear();

        public bool Contains(Hero item) => _heroes.Contains(item);

        public void CopyTo(Hero[] array, int arrayIndex) => _heroes.CopyTo(array, arrayIndex);

        public bool Remove(Hero item) => _heroes.Remove(item);

        public int Count => _heroes.Count;
        public bool IsReadOnly => true;

        #endregion

        
    }
}