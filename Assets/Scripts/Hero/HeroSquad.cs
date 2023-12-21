using System;
using Sirenix.Utilities;
using UnityEngine;
using UniRx;

namespace Tyrant
{
    public class HeroSquad : MonoBehaviour
    {

        public Hero heroPrefab;

        public Hero[] heroes = new Hero[3];
        
        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                 var hero = Instantiate(heroPrefab, new Vector3(1.2f * i, 0, 0), Quaternion.identity, transform);
                 hero.heroName = $"HERO {i}";
                 heroes[i] = hero;
            }

            var times = 0;
            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(v =>
            {
                times++;
                heroes.ForEach(hero =>
                {
                    var a = hero.actionQueue.CanAction(20);
                    if (a == null) return;
                    Debug.Log($"{a.heroName} 行动 at {times}");
                });
            });

        }
    }
}
