using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant.UI;
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
    public class WorkBenchManager : MonoBehaviour, IWorkBenchUIHandler, IWorkBenchRound
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

        private void Monitor()
        {
            // cardInHandStream
            //     // .Skip(1)
            //     .Where(v => v != null)
            //     // .Select(v => v.value)
            //     .Subscribe(v =>
            //     {
            //         
            //         Debug.Log($"#检视# {v.description}");
            //         // if (v == null)
            //         // {
            //         //     Debug.Log($"#检视# 取消选中卡牌");
            //         // }
            //         // else
            //         // {
            //         //     Debug.Log($"#检视# 选中卡牌{v.toolName}");
            //         // }
            //     })
            //     .AddTo(this);
            //
            // checker
            //     .Subscribe(v => Debug.Log(v.description))
            //     .AddTo(this);

            // cardInHandStream
            //     .CombineLatest(checker, (card, checkerboard)
            //         => new CheckerPack(card, checkerboard))
            //     .Where(v => v is {toolWrapper: not null, tool: not null})
            //     .Subscribe(v =>
            //     {
            //         Debug.Log("<<-------");
            //         Debug.Log(v.tool.description);
            //         Debug.Log(v.toolWrapper?.description ?? "");
            //         Debug.Log("------->>");
            //     }).AddTo(this);

            main.CheckerPackStatus()
                .Where(v => v is {toolWrapper: not null, tool: not null})
                // 如果是进入棋盘格
                .Where(v => v.toolWrapper.status == CheckerStatus<WorkBench.ToolWrapper>.Status.Enter)
                // 并且卡牌处于被选中状态
                .Where(v => v.tool.status == CheckerStatus<Tool>.Status.Enter)
                .Subscribe(v =>
            { 
                Debug.Log("添加预览");
                Debug.Log(v.tool.status);

                var toolWrapper = v.toolWrapper.value;

                var tool = v.tool.value;
                var all = tool.diceBuffInfo.buffDataSO
                    .effectOnLocation
                    .AllEffect(toolWrapper.position, workBench.allSlots);

                all.ForEach(v => v.AddPreviewBuff(tool.diceBuffInfo));


                workBench.allSlots.ForEach(v => v.Value.UpdatePreviewBuff());

            }).AddTo(this);

            
            main.CheckerPackStatus()
                .Where(v => v is {toolWrapper: not null, tool: not null})
                .Where(v => v.tool.status != CheckerStatus<Tool>.Status.Empty)
                .Where(v => v.tool.status == CheckerStatus<Tool>.Status.Leave || v.toolWrapper.status == CheckerStatus<WorkBench.ToolWrapper>.Status.Leave)
                .DistinctUntilChanged()
                .Subscribe(v =>
                { 
                    Debug.Log("移除预览");

                    var toolWrapper = v.toolWrapper.value;

                    var tool = v.tool.value;
                    var all = tool.diceBuffInfo.buffDataSO
                        .effectOnLocation
                        .AllEffect(toolWrapper.position, workBench.allSlots);

                    all.ForEach(v => v.RemovePreviewBuff(tool.diceBuffInfo));

                    workBench.allSlots.ForEach(v => v.Value.UpdatePreviewBuff());
                    
                }).AddTo(this);
        }
        
        public struct CheckerPack
        {
            public CheckerStatus<Tool> tool;
            public CheckerStatus<WorkBench.ToolWrapper> toolWrapper;

            public CheckerPack(CheckerStatus<Tool> t, CheckerStatus<WorkBench.ToolWrapper> wrapper)
            {
                tool = t;
                toolWrapper = wrapper;
            }
        }
        
        #endregion


        #region 指向的棋盘格

        
        private ReactiveProperty<CheckerStatus<WorkBench.ToolWrapper>> dd = new ();
        public IObservable<CheckerStatus<WorkBench.ToolWrapper>> checker => dd;//.Where(v => v != null);
        public void EnterCheckerboard(CheckerStatus<WorkBench.ToolWrapper> value)
        {
            dd.Value = value;
            
            if (value.status == CheckerStatus<WorkBench.ToolWrapper>.Status.Leave)
            {
                dd.Value = CheckerStatus<WorkBench.ToolWrapper>.Empty(value.value);
            }
        }
        
        public class CheckerStatus<T> where T: ICheckerStatus
        {
            public Status status;

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

            public CheckerStatus(Status status, T wrapper)
            {
                this.status = status;
                value = wrapper;
            }

            public static CheckerStatus<T> Enter(T toolWrapper) =>
                new CheckerStatus<T>(Status.Enter, toolWrapper);
            public static CheckerStatus<T> Leave(T toolWrapper) =>
                new CheckerStatus<T>(Status.Leave, toolWrapper);
            
            public static CheckerStatus<T> Empty(T toolWrapper) =>
                new CheckerStatus<T>(Status.Empty, toolWrapper);
        }

        public interface ICheckerStatus
        {
            public string debugDescription { get; }
        }
        
        #endregion


        #region 选中的手牌

        /// <summary>
        /// 选中的手牌
        /// </summary>
        private readonly ReactiveProperty<CheckerStatus<Tool>> _toolInHand = new (null);
        /// <summary>
        /// 选中的手牌
        /// </summary>
        public Tool cardInHand => _toolInHand.Value.value;

        public IObservable<CheckerStatus<Tool>> cardInHandStream => _toolInHand;//.Where(v => v != null);

        public ReadOnlyReactiveProperty<IEnumerable<WorkBench.ToolWrapper>> buff => _toolInHand
            .Where(v => v!= null)
            .Select(v => v.value)
            .Select(v =>
            {
                var f = v.diceBuffInfo.buffDataSO.effectOnLocation.AllEffect(dd.Value.value.position, workBench.allSlots)
                    .Select(v => v.toolWrapper);
                return f;
            }).ToReadOnlyReactiveProperty();


        public void ToolIsSelected(CheckerStatus<Tool> tool)
        {
            _toolInHand.Value = tool;
            Debug.Log(tool.status);
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
            // 是否可以放置
            var canPlace = slot.PlaceToolInSlot(cardInfoMono.tool);
            
            if (!canPlace) return;
            
            cardInfoMono.Use();

            workBench.allSlots.ForEach(v =>
            {
                // Debug.Log(v.Value.toolWrapper.position);

                var slot = v.Value;
                Debug.Log("fdaslgsadfsadf");
                
                slot.Configuration();
                
                slot.UpdatePreviewBuff();

            });

            workBench.allSlots.ForEach(v => v.Value.ReScore());
        }

        
        /// <summary>
        /// 安装buff
        /// </summary>
        /// <param name="positions">需要buff的slot</param>
        /// <param name="diceBuffInfo">buff</param>
        public void AddBuffToEachSlot(IEnumerable<Vector2Int> positions, DiceBuffInfo diceBuffInfo)
        {
            workBench.allSlots
                .Where(v => positions.Contains(v.Key.position))
                .ForEach(v => v.Value.AddBuff(diceBuffInfo));
        }
        
        
        
        [ShowInInspector, NonSerialized] public WorkBench workBench;
        [ShowInInspector, NonSerialized] public GameObject workBenchUI;
        public ForgeItem forgeItem;

        public WorkBenchEventSO workBenchEventSO;
        
        [LabelText("工作台Prefab")]
        public GameObject workBenchPrefab;

        public BluePrint bluePrint;
        
        public ToolSO[] toolSos;

        private readonly List<IWorkBenchRound> _allQueues = new();
        
        [ShowInInspector, HideIf("@this.workBench != null")] public int allOccupiedInThisTurn => workBench?
                    .allSlots.Values
                    .Count(v => v.isOccupied) ?? 0;

        [LabelText("每回合最大使用卡牌")]
        public int maxWorkBenchOccupied = 3;
        [ShowInInspector, NonSerialized, LabelText("已熔铸的次数")] 
        public int staminaCost = 0;
        [ShowInInspector, LabelText("可熔铸的次数")] 
        public int staminaMax => Protagonist.main.stamina;

        #region 制作过程
        

        private int allMakesScore => workBench.allMakes
            .Select(v => v.CalculateScore())
            .Sum();

        private int allQualityScore => workBench.allQuality
            .Select(v => v.CalculateScore())
            .Sum();
        
        private void CalculateScore()
        {
            var makes = allMakesScore;
            var qualities = allQualityScore;
            
            
            
            workBenchEventSO.ScoreDidChange(forgeItem.make.power + makes, forgeItem.quality.power + qualities);
            // make.OnNext(makes);
            // quality.OnNext(qualities);
        }
        


        public bool CanBePlaced(ToolOnTable toolOnTable)
        {
            return allOccupiedInThisTurn < maxWorkBenchOccupied;
        }

        public void DidPreviewTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            var slot = workBench.SlotBy(location);
            
            if (slot.isOccupied) return;

            slot.PreviewTool(toolOnTable);

            workBench.NewPreviewBuffTo(location, toolOnTable);

        }

        public void DidPinTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            var slot = workBench.SlotBy(location);

            if (slot.isOccupied) return;
            
            // 先取消预览
            DidUnPreviewTool(location, toolOnTable);
            
            slot.Pin(toolOnTable);
            
            workBench.NewBuffTo(location, toolOnTable);
            
            CalculateScore();
        }

        public void DidUnPinTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            workBench.SlotBy(location).UnPin();
            
            workBench.GetAllEffectPositions(location, toolOnTable)
                .ForEach(v => v.RemoveBuff(toolOnTable.diceBuffInfo));
            
            CalculateScore();
        }

        public void DidUnPreviewTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            var d = workBench.SlotBy(location);
            if (d != null && d.preview.Value)
            {
                workBench.GetAllEffectPositions(location, d.preview.Value)
                    .ForEach(v => v.RemovePreviewBuff(d.preview.Value.diceBuffInfo));
            }
            
            workBench.allSlots
                .ForEach(v => v.Value.UnPreviewTool());
        }


        public void Drag(ToolOnTable toolOnTable)
        {
            
        }
        #endregion


        public void StartAWorkBench(IMaterial[] materials)
        {
            materials.ForEach(v =>
            {
                Debug.Log(v.ToString());
            });
            
            workBench = new WorkBench(bluePrint, materials);
            
            workBenchUI = Instantiate(workBenchPrefab);
            
            // 广播 需要延迟到所有prefab创建完成
            workBenchEventSO.BlueprintDidSelected(bluePrint);

            var toolsBox = workBenchUI.GetComponent<ToolsBox>();
            
            forgeItem = new ForgeItem(bluePrint);
            
            _allQueues.Clear();
            _allQueues.Add(workBench);
            _allQueues.Add(toolsBox);
            
            PrepareNewRound();
            
            NewTurn();
        }
        
        public void DidForgeThisTurn()
        {
            workBenchEventSO.TurnDidEnded();
            staminaCost += 1;

            var makes = allMakesScore;
            var qualities = allQualityScore;
            
            ForgeItemTakePower(makes, qualities);
  
            // 决策是否需要结束或者下一回合
            DetermineIfEndForge();
        }

        private void ForgeItemTakePower(int makes, int qualities)
        {
            forgeItem.NewStrike(new StrikePower(Strike.Shape, makes));
            forgeItem.NewStrike(new StrikePower(Strike.Quality, qualities));
        }

        private void DetermineIfEndForge()
        {
            if (staminaCost < Protagonist.main.stamina)
            {
                NewTurn();
                // 重新计算分数
                CalculateScore();
            }
            else
            {
                DidEndRound();
            }
        }

        

        public void PrepareNewRound()
        {
            workBenchEventSO.PrepareNewRound();
            staminaCost = 0;
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
            workBenchEventSO.NewTurnDidStarted();
            _allQueues.ForEach(v => v.NewTurn());
        }


        [Button]
        public void MockABench()
        {

            bluePrint = BluePrintGenesis.main.allBlueprints.First();

            workBench = new WorkBench(bluePrint, new IMaterial[] {  });

            // StartAWorkBench(new IMaterial[] {  });
            // 生成棋盘
            WorkBenchBoardUI.main.GenerateBoard(bluePrint);

        }
    }
}
