using System;
using System.Collections;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class HeroSquad : MonoBehaviour
    {

        public JobSO[] jobSO;
        
        [NonSerialized, ShowInInspector] public readonly HeroMono[] heroes = new HeroMono[3];

        public Action<EnemyMono> enemyDefeated;

        [NonSerialized, ShowInInspector] private BattleStands battles;


        public TextMeshProUGUI dungeonLabel;
        public TextMeshProUGUI dungeonRoadMapLabel;
        public HeroSquadBackpack backpack;

        public Canvas parentCanvas;
        
        private void Start()
        {
            for (var i = 0; i < jobSO.Length; i++)
            {
                 var hero = Instantiate(jobSO[i].heroMonoPrefab, new Vector3(1.2f * i, 0, 0), Quaternion.identity, transform);
                 hero.transform.localPosition = new Vector3(1.6f * i, 0, 0);
                 hero.RestoreFromSO(jobSO[i]);
                 heroes[i] = hero;
            }

            var postion = Camera.main.GetCanvasPosition(transform, parentCanvas);

            backpack.GetComponent<RectTransform>().anchoredPosition = 
                postion + new Vector2(-24, -35);
            
            dungeonLabel.transform.localPosition = postion +  new Vector2(-24, -25);
            
            dungeonRoadMapLabel.transform.localPosition = postion +  new Vector2(95, -25);
            //95 20
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            
            Gizmos.DrawLine(Vector3.zero, new Vector3(4, 0, 0));

            var c = Camera.main.WorldToScreenPoint(transform.position);
            
            Gizmos.DrawCube(c, Vector3.one);
        }

        [ShowInInspector]
        private Dungeon _dungeon;

        // 开始`Dungeon`
        public void DidEnterDungeon(Dungeon dungeon)
        {
            _dungeon = dungeon;
            dungeon.DidStartDungeon(this);
            GoToNextDungeonNode();
        }

        // 往下走
        private void GoToNextDungeonNode()
        {
            var e = _dungeon.StartANewNode(this);

            dungeonRoadMapLabel.text = _dungeon.roadMap;//"o-o-oo-o-o";
            // 如果还有node
            if (e is DungeonTripNode node)
            {
                var enemySO = node.enemySO; 
                _enemyMono = Instantiate(enemySO.enemyMonoPrefab, new Vector3(5.2f, 0, 0), Quaternion.identity, transform);
                _enemyMono.NewEnemy(enemySO);
                NewEnemy(_enemyMono, node);
            }
            
            if (e is DungeonHealthSupplyTripNode supply)
            {
                _dungeon.DidFinishThisNode(this);
                DelayNextNode(supply);
            }
        }

        private EnemyMono _enemyMono;
        public void NewEnemy(EnemyMono enemyMono, IDungeonNode dungeonNode)
        {

            _enemyMono = enemyMono;
            
            _enemyMono.transform.localPosition = new Vector3(5.2f, 0, 0);
            
            battles = new BattleStands(new BattlePosition(heroes.Reverse()), new BattlePosition(new []{ _enemyMono }));
            
            StartCoroutine(StartBattle(0, 1, dungeonNode));

        }
        
        private void EnemyDidDefeated(EnemyMono eMono, IDungeonNode dungeonNode)
        {
            enemyDefeated.Invoke(_enemyMono);
            var rawMaterials = _enemyMono.enemy.loot.Select(v => v.toRawMaterial.toMaterial);
            rawMaterials.ForEach(backpack.AddItem);

            _dungeon.DidFinishThisNode(this);
            
            DelayNextNode(dungeonNode);
        }

        private void DelayNextNode(IDungeonNode dungeonNode)
        {
            Observable.Timer(TimeSpan.FromSeconds(dungeonNode.delay))
                .Take(1)
                .Subscribe(v => GoToNextDungeonNode())
                .AddTo(this);
        }

        
        private IEnumerator StartBattle(int index, int turn, IDungeonNode dungeonNode)
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
            
                StartCoroutine(StartBattle(index, turn, dungeonNode));
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
                    yield return new WaitForSeconds(0.5f);
                    StartCoroutine(StartBattle(index, turn, dungeonNode));
                }
                else
                {
                    yield return null;
                    EnemyDidDefeated(_enemyMono, dungeonNode);
                }
                
            }
        }
    }
}
