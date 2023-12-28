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
        public CharacterSO[] characterSos;
        
        [ShowInInspector] public HeroMono[] heroes;


        public HeroMono heroMonoPrefab;
        
        public EnemyMono enemyMonoPrefab;
        // {
        //     get
        //     {
        //         if (!_heroes.IsNullOrEmpty()) return _heroes;
        //         _heroes = new HeroMono[3];
        //         M();
        //         return _heroes;
        //     }
        // }
        //
        // private HeroMono[] _heroes;// = new HeroMono[3];
        
        public Action<EnemyMono> enemyDefeated;

        [NonSerialized, ShowInInspector] private BattleStands battles;


        public TextMeshProUGUI dungeonLabel;
        public TextMeshProUGUI dungeonRoadMapLabel;
        public HeroSquadBackpack backpack;

        public Canvas parentCanvas;

        private void Start()
        {
            parentCanvas.worldCamera = Camera.main;

            PositionCanvas();
        }

        private void PositionCanvas()
        {
            var position = Camera.main.GetCanvasPosition(transform, parentCanvas);

            backpack.GetComponent<RectTransform>().anchoredPosition = 
                position + new Vector2(0, -35);
            
            dungeonLabel.transform.localPosition = position +  new Vector2(0, -25);
            
            dungeonRoadMapLabel.transform.localPosition = position +  new Vector2(115, -25);
        }

        private void OnEnable()
        {
            M();
        }

        private void M()
        {
            if (!heroes.IsNullOrEmpty())return;

            heroes = new HeroMono[3];
            
            for (var i = 0; i < jobSO.Length; i++)
            {
                 var hero = Instantiate(heroMonoPrefab, new Vector3(1.2f * i, 0, 0), Quaternion.identity, transform);
                 hero.transform.localPosition = new Vector3(1f * i + 0.5f, 0, 0);
                 hero.RestoreFromSO(characterSos[i], jobSO[i]);
                 heroes[i] = hero;
            }
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
            if (_dungeon != null) return;
            _dungeon = dungeon;
            dungeon.DidStartDungeon(this);
            GoToNextDungeonNode();
        }

        // 往下走
        private void GoToNextDungeonNode()
        {
            var e = _dungeon.StartANewNode(this);

            dungeonRoadMapLabel.text = _dungeon.roadMap;//"o-o-oo-o-o";
            
            if (e == null)
            {
                Debug.Log("Dungeon 完成");
                _dungeon = null;
                return;
            }

            // 如果还有node
            if (e is DungeonTripNode node)
            {
                var enemySO = node.enemySO; 
                _enemyMono = Instantiate(enemyMonoPrefab, new Vector3(5.2f, 0, 0), Quaternion.identity, transform);
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
            
            _enemyMono.transform.localPosition = new Vector3(4.3f, 0, 0);
            
            battles = new BattleStands(new BattlePosition(heroes.Reverse()), new BattlePosition(new []{ _enemyMono }));

            StartBattle(0, 1, dungeonNode);

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
            if (AdventureManager.main.adventurePlayFast)
            {
                GoToNextDungeonNode();
                return;
            }
            
            Observable.Timer(TimeSpan.FromSeconds(dungeonNode.delay))
                .Take(1)
                .Subscribe(_ => GoToNextDungeonNode())
                .AddTo(this);
        }

        
        private void StartBattle(int index, int turn, IDungeonNode dungeonNode)
        {
            if (index >= battles.Count)
            {
                index = 0;
                turn += 1;
            }

            var hero = battles.ElementAt(index);

            var canAction = hero.actionQueue.CanAction(20);
    
            ++index;
            
            if (!canAction)
            {
                StartBattle(index, turn, dungeonNode);
            }
            else
            {
                
                hero.Attack(hero.heroic is Hero ? _enemyMono : battles.heroes.front);

                Debug.Log($"{hero} 行动 攻击{hero} at {turn}");

                if (battles.heroes.isAllDown)
                {
                    return;
                }
                
                if (_enemyMono.stillAlive)
                {

                    if (AdventureManager.main.adventurePlayFast)
                    {
                        StartBattle(index, turn, dungeonNode);
                    }
                    else
                    {
                        Observable.Timer(TimeSpan.FromSeconds(0.5f))
                            .Take(1)
                            .Subscribe(_ => StartBattle(index, turn, dungeonNode))
                            .AddTo(this);
                    }
                    
                }
                else
                {
                    Observable.Timer(TimeSpan.FromSeconds(1f))
                        .Take(1)
                        .Subscribe(_ => EnemyDidDefeated(_enemyMono, dungeonNode))
                        .AddTo(this);
                    
                }
                
            }
        }
    }
}
