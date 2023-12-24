using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class HeroSquad : MonoBehaviour
    {

        public JobSO[] jobSO;
        
        [NonSerialized, ShowInInspector] public readonly HeroMono[] heroes = new HeroMono[3];

        public Action<EnemyMono> enemyDefeated;
        
        
        // [NonSerialized]
        // private List<IAmHero> allBattles = new();

        [NonSerialized, ShowInInspector] private BattleStands battles;
        
        private void Start()
        {
            for (var i = 0; i < jobSO.Length; i++)
            {
                 var hero = Instantiate(jobSO[i].heroMonoPrefab, new Vector3(1.2f * i, 0, 0), Quaternion.identity, transform);
                 hero.transform.localPosition = new Vector3(1.6f * i, 0, 0);
                 hero.RestoreFromSO(jobSO[i]);
                 heroes[i] = hero;
            }
        }


        private EnemyMono _enemyMono;
        public void NewEnemy(EnemyMono enemyMono)
        {

            _enemyMono = enemyMono;

            _enemyMono.transform.localPosition = new Vector3(5.2f, 0, 0);
            
            
            // allBattles.Clear();
            //
            // allBattles.AddRange(heroes);
            // allBattles.Add(_enemyMono);


            battles = new BattleStands(new BattlePosition(heroes.Reverse()), new BattlePosition(new []{ _enemyMono }));
            
            StartCoroutine(StartBattle(0, 1));

        }
        
        private IEnumerator StartBattle(int index, int turn)
        {
            if (index >= battles.Count)
            {
                index = 0;
                turn += 1;
            }

            var hero = battles.ElementAt(index);//[index];
            
            var canAction = hero.actionQueue.CanAction(20);
    
            ++index;
            
            if (!canAction)
            {
                yield return new WaitForFixedUpdate();
            
                StartCoroutine(StartBattle(index, turn));
            }
            else
            {


                if (hero.heroic is Hero)
                {
                    hero.Attack(_enemyMono);
                }
                else
                {
                    hero.Attack(battles.heroes.front);
                }
            
                Debug.Log($"{hero} 行动 攻击{hero} at {turn}");
            
                if (_enemyMono.stillAlive)
                {
                    yield return new WaitForSeconds(1);
                    StartCoroutine(StartBattle(index, turn));
                }
                else
                {
                    yield return null;
                    enemyDefeated.Invoke(_enemyMono);
                }
                
            }
        }
    }
}
