using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant.UI;
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
            }
            else
            {
                Destroy(this);
            }
        }
        
        #endregion
        
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
    }
}
