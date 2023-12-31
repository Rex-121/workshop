using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant.UI;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class WorkBenchManager : MonoBehaviour, IWorkBenchUIHandler, IWorkBenchRound
    {
        #region 单例

        public static WorkBenchManager main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        
        #endregion
        
        [ShowInInspector, NonSerialized] public WorkBench workBench;
        [ShowInInspector, NonSerialized] public GameObject workBenchUI;
        [ShowInInspector, NonSerialized] public int staminaCost = 0;
        public ForgeItem forgeItem;
        
        [LabelText("工作台Prefab")]
        public GameObject workBenchPrefab;

        public BluePrintSO bluePrintSO;
        
        public ToolSO[] toolSos;

        private readonly List<IWorkBenchRound> _allQueues = new();
        
        [ShowInInspector, HideIf("@this.workBench != null")] public int allOccupiedInThisTurn => workBench?
                    .allSlots.Values
                    .Count(v => v.isOccupied) ?? 0;

        [LabelText("每回合最大使用灵感数")]
        public int maxWorkBenchOccupied = 3;

        #region 制作过程
        
        
        // public WorkBenchUI workBenchUI;
        public readonly BehaviorSubject<int> make = new(0);
        public readonly BehaviorSubject<int> quality = new(0);


        private int allMakesScore => workBench.allMakes
            .Select(v => v.CalculateScore())
            .Sum();

        private int allQualityScore => workBench.allQuality
            .Select(v => v.CalculateScore())
            .Sum();
        
        private void CalculateScore()
        {
            make.OnNext(allMakesScore);
            quality.OnNext(allQualityScore);
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
            // if (toolOnTable == null)
            // {
            //     // 清空所有的preview
            // workBench.allSlots.Values.ForEach(v => v.ReleasePreviewBuff());
            // }
            // else
            // {
            //     // 清空所有的buff
            //     // workBench.allSlots.Values.ForEach(v => v.ReleaseBuffBy(toolOnTable.tool.id));
            // }
        }
        #endregion


        [Button]
        public void StartAWorkBench()
        {

            var bluePrint = BluePrint.FromSO(bluePrintSO);
            
            workBench = new WorkBench(bluePrint);

            workBenchUI = Instantiate(workBenchPrefab);

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
            Destroy(workBenchUI);
            workBench = null;
            forgeItem = null;
            _allQueues.Clear();
        }


        public void NewTurn()
        {
            _allQueues.ForEach(v => v.NewTurn());
        }
    }
}
