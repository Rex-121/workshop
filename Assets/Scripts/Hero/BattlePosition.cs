using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Tyrant
{

    public class BattleStands: ICollection<IAmHero>
    {
        [ShowInInspector]
        public BattlePosition heroes;

        public BattlePosition enemies;

        private List<IAmHero> f;

        public BattleStands(BattlePosition heroes, BattlePosition enemies)
        {
            this.heroes = heroes;
            this.enemies = enemies;
            f = new();
            f.AddRange(this.heroes);
            f.AddRange(this.enemies);
        }

        public IEnumerator<IAmHero> GetEnumerator()
        {
            return f.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IAmHero item)
        {
            
        }

        public void Clear()
        {
            heroes.Clear();
            enemies.Clear();
        }

        public bool Contains(IAmHero item)
        {
            return f.Contains(item);
        }

        public void CopyTo(IAmHero[] array, int arrayIndex)
        {
            f.CopyTo(array, arrayIndex);
        }

        public bool Remove(IAmHero item)
        {
            return f.Remove(item);
        }

        public int Count => f.Count;
        public bool IsReadOnly => true;
    }
    
    public readonly struct BattlePosition: ICollection<IAmHero>
    {
        [ShowInInspector, LabelText("前置")]
        public IAmHero front
        {
            get
            {
                var first = f.First();

                return first is not {stillAlive: true} ? middle : first;
            }
        }

        [ShowInInspector, LabelText("中间")]
        public IAmHero middle {
            get
            {
                var second = f[1];

                return second is not {stillAlive: true} ? tail : second;
            }
        }

        [ShowInInspector, LabelText("后置")]
        public IAmHero tail => f[2];

        private readonly List<IAmHero> f;// = new();

        public BattlePosition(IEnumerable<IAmHero> heroes)
        {
            f = new();
            f.AddRange(heroes);
        }
        
        public IEnumerator<IAmHero> GetEnumerator()
        {
            return f.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return f.GetEnumerator();
        }

        public void Add(IAmHero item)
        {
            if (f.Count() >= 3) return;
            f.Add(item);
        }

        public void Clear()
        {
            f.Clear();
        }

        public bool Contains(IAmHero item)
        {
            return f.Contains(item);
        }

        public void CopyTo(IAmHero[] array, int arrayIndex)
        {
            f.CopyTo(array, arrayIndex);
        }

        public bool Remove(IAmHero item)
        {
            return f.Remove(item);
        }

        public int Count => f.Count;

        public bool IsReadOnly => true;
    }
}