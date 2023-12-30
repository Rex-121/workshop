using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant.UI;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class WorkBenchManager : MonoBehaviour, IWorkBenchUIHandler
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
        
        [ShowInInspector, NonSerialized] public WorkBench workBench;// = new WorkBench();

        [LabelText("工作台Prefab")]
        public GameObject workBenchPrefab;

        public BluePrintSO bluePrintSO;
        
        public ToolSO[] toolSos;
        
        
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
            
            workBench = new WorkBench(BluePrint.FromSO(bluePrintSO));
            
            Instantiate(workBenchPrefab);
            
        }
        public void DidForgeThisTurn()
        {
            Debug.Log("End Forge!");

            var makes = allMakesScore;
            var qualities = allQualityScore;
            
            Debug.Log($"make={makes}, quality={qualities}");
            
            // 合成
            var equipment = EquipmentGenesis.main.DoCraft(makes, qualities, bluePrintSO);
            
            InventoryManager.main.AddItem(equipment);
            
            // 清空棋盘
            workBench.DidForgeThisTurn();
            
            // 重新计算分数
            CalculateScore();
        }
        
    }
}
