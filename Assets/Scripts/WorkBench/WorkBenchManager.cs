using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using WorkBench;

namespace Tyrant
{
    /*
     * 1. 准备蓝图
     * 2. PrepareNewRound
     * 3. NewTurn
     * 4. DidForgeThisTurn
     * 5. 计算分数 ♾️️
     */
    public class WorkBenchManager : MonoBehaviour, IWorkBenchRound
    {
        #region 单例

        public static WorkBenchManager main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                Monitor();
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// 监控鼠标是否进入棋盘格
        /// 查看哪些棋盘格需要预览buff
        /// </summary>
        private void Monitor()
        {
            // 监控鼠标是否进入棋盘格
            main.CheckerPackStatus()
                .Where(v => v is {toolWrapper: not null, tool: not null})
                // 如果是进入棋盘格
                .Where(v => v.toolWrapper.status == CheckerStatus<WorkBench.ToolWrapper>.Status.Enter)
                // 并且卡牌处于被选中状态
                .Where(v => v.tool.status == CheckerStatus<Tool>.Status.Enter)
                .Subscribe(v =>
            { 
                var toolWrapper = v.toolWrapper.value;
                var tool = v.tool.value;
                var all = tool.diceBuffInfo.buffDataSO
                    .AllEffect(toolWrapper.position, workBench.allSlots);

                all.ForEach(x => x.PreviewBuff(true, tool.diceBuffInfo));
            }).AddTo(this);

            // 查看哪些棋盘格需要预览buff
            main.CheckerPackStatus()
                .Where(v => v is {toolWrapper: not null, tool: not null})
                .Where(v => v.tool.status != CheckerStatus<Tool>.Status.Empty)
                .Where(v => v.tool.status == CheckerStatus<Tool>.Status.Leave || v.toolWrapper.status == CheckerStatus<WorkBench.ToolWrapper>.Status.Leave)
                .DistinctUntilChanged()
                .Subscribe(v =>
                { 
                    var toolWrapper = v.toolWrapper.value;
                    var tool = v.tool.value;
                    var all = tool.diceBuffInfo.buffDataSO
                        .AllEffect(toolWrapper.position, workBench.allSlots);
                    all.ForEach(x => x.PreviewBuff(false, tool.diceBuffInfo));
                }).AddTo(this);
        }

        #endregion


        #region 指向的棋盘格
        
        private readonly ReactiveProperty<CheckerStatus<WorkBench.ToolWrapper>> _pointInChecker = new ();
        public IObservable<CheckerStatus<WorkBench.ToolWrapper>> checker => _pointInChecker;
        public void EnterCheckerboard(CheckerStatus<WorkBench.ToolWrapper> value)
        {
            _pointInChecker.Value = value;
            
            if (value.status == CheckerStatus<WorkBench.ToolWrapper>.Status.Leave)
            {
                // 如果是移开棋盘格，离开后置为空
                _pointInChecker.Value = CheckerStatus<WorkBench.ToolWrapper>.Empty(value.value);
            }
        }
        
        #endregion


        #region 选中的手牌

        /// <summary>
        /// 选中的手牌
        /// </summary>
        private readonly ReactiveProperty<CheckerStatus<Tool>> _toolInHand = new (null);

        public IObservable<CheckerStatus<Tool>> cardInHandStream => _toolInHand;

        /// <summary>
        /// 当有卡牌被选中
        /// </summary>
        /// <param name="tool">卡牌</param>
        public void ToolIsSelected(CheckerStatus<Tool> tool)
        {
            _toolInHand.Value = tool;
            if (tool.status == CheckerStatus<Tool>.Status.Leave)
            {
                _toolInHand.Value = CheckerStatus<Tool>.Empty(tool.value);
            }
        }

        #endregion


        /// <summary>
        /// 在指定的棋盘格中使用工具
        /// </summary>
        /// <param name="cardInfoMono">工具</param>
        /// <param name="slot">棋盘格</param>
        public void UseToolOnSlot(CardInfoMono cardInfoMono, WorkBenchSlot slot)
        {
            if (!CanBePlaced()) return;
            
            // 是否可以放置
            var canPlace = slot.PlaceToolInSlot(cardInfoMono.tool);
            
            if (!canPlace) return;
            
            cardInfoMono.Use();

            var buff = slot.tool.diceBuffInfo;
                
            // 其他受影响的slot position
            var allEffectSlots = buff
                .buffDataSO
                .effectOnLocation.EffectedOnSlots(slot);
                
            AddBuffToEachSlot(allEffectSlots, buff);

            workBench.allSlots.ForEach(v => v.Value.Recalculate());
            
            WorkBenchBoardUI.main.UpdateInformation();
        }
        
        
        /// <summary>
        /// 安装buff
        /// </summary>
        /// <param name="positions">需要buff的slot</param>
        /// <param name="diceBuffInfo">buff</param>
        private void AddBuffToEachSlot(IEnumerable<Vector2Int> positions, DiceBuffInfo diceBuffInfo)
        {
            workBench.allSlots
                .Where(v => positions.Contains(v.Key.position))
                .ForEach(v => v.Value.AddBuff(diceBuffInfo));
        }
        
        
        /// <summary>
        /// 当前的回合数
        /// </summary>
        private int _currentTurn;


        [ShowInInspector, NonSerialized]
        public WorkBench workBench;
        
       
        
        [ShowInInspector]
        public ForgeItem forgeItem;

        public WorkBenchEventSO workBenchEventSO;
        
        public BluePrint bluePrint;
        
        private readonly List<IWorkBenchRound> _allQueues = new();
        
        [ShowInInspector, ShowIf("@this.workBench != null"), LabelText("已使用的卡牌数")] 
        public int allOccupiedInThisTurn => workBench?
                    .allSlots.Values
                    .Count(v => v.isOccupied) ?? 0;

        [LabelText("每回合最大使用卡牌")]
        public int maxWorkBenchOccupied = 3;

        [ShowInInspector, NonSerialized, LabelText("已熔铸的次数")]
        public int staminaCost;
        [ShowInInspector, LabelText("可熔铸的次数")] 
        public int staminaMax => Protagonist.main.stamina;

        [ShowInInspector, NonSerialized]
        [BoxGroup("Prefab")]
        public GameObject workBenchUI;
        
        [BoxGroup("Prefab"), LabelText("卡牌的曲线")]
        public CurveForCard curveForCard;
        
        [LabelText("工作台Prefab")]
        [BoxGroup("Prefab")]
        public GameObject workBenchPrefab;
        
        #region 制作过程
        

        private int allMakesScore => workBench.allMakes
            .Select(v => v.CalculateScore())
            .Sum();

        private int allQualityScore => workBench.allQuality
            .Select(v => v.CalculateScore())
            .Sum();
        
        private bool CanBePlaced()
        {
            return allOccupiedInThisTurn < maxWorkBenchOccupied;
        }
        
        #endregion

        /// <summary>
        /// 更换状态
        /// </summary>
        /// <param name="status">状态</param>
        private void TryChangeStatus(WorkBenchStatus status)
        {
            status.Log();
        }

        public void StartAWorkBench(IMaterial[] materials)
        {
            // 开始锻造
            TryChangeStatus(WorkBenchStatus.Begin);
            
            materials.ForEach(v =>
            {
                Debug.Log(v.ToString());
            });
            
            workBench = new WorkBench(bluePrint, materials);
            
            TryChangeStatus(WorkBenchStatus.AddMaterial);
            
            workBenchUI = Instantiate(workBenchPrefab);
            
            WorkBenchBoardUI.main.GenerateBoard(bluePrint);
            
            // 广播 需要延迟到所有prefab创建完成
            workBenchEventSO.BlueprintDidSelected(bluePrint);
            
            forgeItem = new ForgeItem(bluePrint);
            
            _allQueues.Clear();
            _allQueues.Add(workBench);
            // _allQueues.Add(toolsBox);
            
            PrepareNewRound();
            
            NewTurn();
        }
        
        
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="amount">数量</param>
        /// <returns>抽出的卡牌</returns>
        public void DrawCardsIfNeeded(int amount)
        {
            DrawCards.main.DrawCardsWithAnimation(amount)
                .Subscribe()
                .AddTo(this);
        }
        public void DrawCardsWithStrategy(IDrawCardStrategy delay)
        {
            delay.readyToDraw
                .Take(1)
                .Subscribe(v => DrawCardsIfNeeded(v.amount))
                .AddTo(this);
        }

        [Button]
        public void DidForgeThisTurn()
        {
            // 计算分数
            ForgeItemTakePower();
            
            // 清理棋盘
            workBench.ClearSlots();
            
            workBenchEventSO.TurnDidEnded();
            
            staminaCost += 1;
            
            // 决策是否需要结束或者下一回合
            DetermineIfEndForge();
        }

        private void ForgeItemTakePower()
        {
            
            var makes = allMakesScore;
            var qualities = allQualityScore;
            
            forgeItem.NewStrike(new StrikePower(Strike.Shape, makes));
            forgeItem.NewStrike(new StrikePower(Strike.Quality, qualities));
            
            workBenchEventSO.ScoreDidChange(forgeItem.make.power + makes, forgeItem.quality.power + qualities);

        }

        private void DetermineIfEndForge()
        {
            if (staminaCost < Protagonist.main.stamina)
            {
                NewTurn();
            }
            else
            {
                DidEndRound();
            }
        }

        

        public void PrepareNewRound()
        {
            _currentTurn = 0;
            staminaCost = 0;
            
            workBenchEventSO.PrepareNewRound();
            
            _allQueues.ForEach(v => v.PrepareNewRound());
        }
        

        // 打造结束
        public void DidEndRound()
        {
            
            // 合成
            var equipment = forgeItem.DoForge();

            InventoryManager.main.AddItem(equipment);
            _allQueues.ForEach(v => v.DidEndRound());
            
            workBenchEventSO.RoundDidEnd();
            
            Destroy(workBenchUI);
            workBench = null;
            forgeItem = null;
            _allQueues.Clear();
        }


        public void NewTurn()
        {
            workBenchEventSO.NewTurnDidStarted(++ _currentTurn);
            _allQueues.ForEach(v => v.NewTurn());
        }


        [Button]
        public void MockABench()
        {

            bluePrint = BluePrintGenesis.main.allBlueprints.First();
            
            workBench = new WorkBench(bluePrint, new IMaterial[] {  });

            Instantiate(workBenchPrefab);
            // 生成棋盘
            WorkBenchBoardUI.main.GenerateBoard(bluePrint);

        }

        #region Pack结构体
        
        public interface ICheckerStatus
        {
            public string debugDescription { get; }
        }

        public readonly struct CheckerPack
        {
            public readonly CheckerStatus<Tool> tool;
            public readonly CheckerStatus<WorkBench.ToolWrapper> toolWrapper;

            public CheckerPack(CheckerStatus<Tool> t, CheckerStatus<WorkBench.ToolWrapper> wrapper)
            {
                tool = t;
                toolWrapper = wrapper;
            }
        }

        public class CheckerStatus<T> where T: ICheckerStatus
        {
            public readonly Status status;

            public enum Status
            {
                Enter, Leave, Empty
            }

            public string description
            {
                get
                {
                    return status switch
                    {
                        Status.Enter => $"#检视# 进入 {value.debugDescription}",
                        Status.Leave => $"#检视# 离开 {value.debugDescription}",
                        Status.Empty => $"#检视# 空 {value.debugDescription}",
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            }

            public T value;

            private CheckerStatus(Status status, T wrapper)
            {
                this.status = status;
                value = wrapper;
            }

            public static CheckerStatus<T> Enter(T toolWrapper) =>
                new (Status.Enter, toolWrapper);
            public static CheckerStatus<T> Leave(T toolWrapper) =>
                new (Status.Leave, toolWrapper);
            
            public static CheckerStatus<T> Empty(T toolWrapper) =>
                new (Status.Empty, toolWrapper);
        }

        #endregion
    }
}
