using System;
using System.Collections;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tyrant
{
    public class HeroSquadMono : MonoBehaviour
    {
        
        // [ShowInInspector, PropertyOrder(-100)] private Hero[] h => heroes.Select(v => v.heroic as Hero).ToArray();


        public int indexInAdventure;

        // public JobSO[] jobSO;
        // public CharacterSO[] characterSos;
        
        [ShowInInspector] public HeroMono[] heroes;

        [ShowInInspector]
        private HeroSquad _squad;

        public HeroMono heroMonoPrefab;
        
        public EnemyMono enemyMonoPrefab;
        private EnemyMono _enemyMono;
        public Action<EnemyMono> enemyDefeated;
        public Action<HeroSquadMono, Dungeon> dungeonDidFinished;
        [NonSerialized, ShowInInspector] private BattleStands battles;

        public HeroInfoDisplay heroInfoDisplay;

        #region CanvasUI
        [BoxGroup("CanvasUI")]
        public TextMeshProUGUI dungeonLabel;
        [BoxGroup("CanvasUI")]
        public TextMeshProUGUI dungeonRoadMapLabel;
        [BoxGroup("CanvasUI")]
        public RectTransform dungeonInfo;
        [FormerlySerializedAs("backpack")] [BoxGroup("CanvasUI")]
        public HeroSquadBackpackMono backpackMono;
        [BoxGroup("CanvasUI")]
        public Canvas parentCanvas;
        [BoxGroup("CanvasUI")]
        public Vector2 canvasOffset = new Vector2(-32, -20);
        
        // 重置UI位置
        private void PositionCanvas()
        {
            parentCanvas.worldCamera = Camera.main;
            var position = Camera.main.GetCanvasPosition(transform.position, parentCanvas);
            dungeonInfo.transform.localPosition = position + canvasOffset;
        }
        #endregion
        
        private void Start()
        {
            PositionCanvas();
        }
        

        public void NewSquad(HeroSquad squad)
        {
            this._squad = squad;
            heroes = new HeroMono[3];

            var squadHeroes = squad.heroes;
            
            for (var i = 0; i < squadHeroes.Length; i++)
            {
                 var hero = Instantiate(heroMonoPrefab, new Vector3(1.2f * i, 0, 0), Quaternion.identity, transform);
                 hero.transform.localPosition = new Vector3(1f * i + 0.5f, 0, 0);
                 hero.hero = squadHeroes.ElementAt(i);
                 hero.heroInfoDisplay += DisplayHeroInfo;
                 heroes[i] = hero;
            }

            backpackMono.squadInventory = squad.inventory;
        }

        // 展示信息
        private void DisplayHeroInfo(Hero hero)
        {
            var display = hero != heroInfoDisplay.hero;
            heroInfoDisplay.gameObject.SetActive(display);
            heroInfoDisplay.hero = display ? hero : null;
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

        #region 战斗

        private void DungeonDidFinished()
        {
            dungeonDidFinished?.Invoke(this, _dungeon);

            _dungeon = null;

            dungeonDidFinished = null;
        }
        
        // 往下走
        private void GoToNextDungeonNode()
        {
            var e = _dungeon.StartANewNode(this);

            dungeonRoadMapLabel.text = _dungeon.roadMap;
            
            if (e == null)
            {
                DungeonDidFinished();
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

        
        public void NewEnemy(EnemyMono enemyMono, IDungeonNode dungeonNode)
        {

            _enemyMono = enemyMono;
            
            _enemyMono.transform.localPosition = new Vector3(4.3f, 0, 0);
            
            battles = new BattleStands(new BattlePosition(heroes.Reverse()), new BattlePosition(new []{ _enemyMono }));

            StartCoroutine(StartBattle(0, 1, dungeonNode));

        }
        
        private void EnemyDidDefeated(EnemyMono eMono, IDungeonNode dungeonNode)
        {
            enemyDefeated.Invoke(_enemyMono);
            var rawMaterials = _enemyMono.enemy.loot.Select(v => v.toRawMaterial.toMaterial);
            // rawMaterials.ForEach(backpack.AddItem);
            rawMaterials.ForEach(_squad.CollectItem);
            

            _dungeon.DidFinishThisNode(this);
            
            DelayNextNode(dungeonNode);
        }

        private void DelayNextNode(IDungeonNode dungeonNode)
        {
            BattleDidEnd();
            
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


        public void BattleDidEnd()
        {
            heroes.ForEach(v => v.BattleDidEnd());
        }
        
        private IEnumerator StartBattle(int index, int turn, IDungeonNode dungeonNode)
        {
            if (index >= battles.Count)
            {
                index = 0;
                turn += 1;
            }

            var hero = battles.ElementAt(index);

            var canAction = hero.actionQueue.CanAction(20) && !hero.heroic.health.isEmpty;
    
            ++index;
            
            if (!canAction)
            {
                yield return null;
                StartCoroutine(StartBattle(index, turn, dungeonNode));
            }
            else
            {
                
                hero.Attack(hero.heroic is Hero ? _enemyMono : battles.heroes.front);

                // Debug.Log($"{hero} 行动 攻击{hero} at {turn}");

                // 全员阵亡
                if (battles.heroes.isAllDown)
                {
                    yield break;
                }
                
                if (_enemyMono.stillAlive)
                {

                    if (AdventureManager.main.adventurePlayFast)
                    {
                        yield return null;
                        StartCoroutine(StartBattle(index, turn, dungeonNode));
                    }
                    else
                    {

                        yield return new WaitForSeconds(1);
                        
                        StartCoroutine(StartBattle(index, turn, dungeonNode));
                        // Observable.Timer(TimeSpan.FromSeconds(0.5f))
                        //     .Take(1)
                        //     .Subscribe(_ => StartBattle(index, turn, dungeonNode))
                        //     .AddTo(this);
                    }
                    
                }
                else
                {
                    yield return null;

                    EnemyDidDefeated(_enemyMono, dungeonNode);
                    // Observable.Timer(TimeSpan.FromSeconds(1f))
                    //     .Take(1)
                    //     .Subscribe(_ => EnemyDidDefeated(_enemyMono, dungeonNode))
                    //     .AddTo(this);

                }
                
            }
        }
        
        #endregion
    }
}
