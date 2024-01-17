using System;
using System.Linq;
using UniRx;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorkBench;

namespace Tyrant.UI
{
    public class WorkBenchUI: MonoBehaviour, IWorkBenchUIHandler
    {

        public GameObject cellOnWorkBenchPrefab;

        public Transform tablePanel;

        private static WorkBench workBench => WorkBenchManager.main.workBench;
        
        public TextMeshProUGUI makeText;
        public TextMeshProUGUI qualityText;

        public TextMeshProUGUI staminaUsageLabel, cardsUsageLabel;
        
        [BoxGroup("进度条")]
        [LabelText("制作进度条")]
        public WorkBenchMakeProgressBar makeProgressBar;
        [BoxGroup("进度条")]
        [LabelText("品质进度条")]
        public WorkBenchQualityProgressBar qualityProgressBar;
        private void Start()
        {
            
            var list = workBench.Start();
            
            GetComponent<GridLayoutGroup>().constraintCount = _bluePrint.boardLines.First().Count();
            
            for (var i = 0; i < list.Count; i++)
            {
                var slot = list[i];
                var gb = Instantiate(cellOnWorkBenchPrefab, tablePanel);
                var cell = gb.GetComponent<SlotOnWorkBench>();
                cell.handler = this;
                cell.SetCellPosition(slot);
            }
        }

        public WorkBenchEventSO workBenchEventSO;


        private void OnEnable()
        {
            workBenchEventSO.scoreDidChange += DisplayInformation;
            workBenchEventSO.blueprintDidSelected += BlueprintInformation;
            workBenchEventSO.newTurnDidStarted += NewTurnDidStarted;
        }
        

        [ShowInInspector]
        private BluePrint _bluePrint;
        private void BlueprintInformation(BluePrint bluePrint)
        {
            _bluePrint = bluePrint;
        }

        private void OnDisable()
        {
            workBenchEventSO.scoreDidChange -= DisplayInformation;
            workBenchEventSO.blueprintDidSelected -= BlueprintInformation;
            workBenchEventSO.newTurnDidStarted -= NewTurnDidStarted;
        }

        private void NewTurnDidStarted(int arg0)
        {
            staminaUsageLabel.text = $"{WorkBenchManager.main.staminaCost}/{WorkBenchManager.main.staminaMax}";
        }

        // 显示信息
        private void DisplayInformation(int make, int quality)
        {
            makeText.text = $"{make}/{_bluePrint.make}";
            qualityText.text = $"{quality}/{_bluePrint.quality}";

            makeProgressBar.Predict(make / (_bluePrint.make * 1.0f));
            qualityProgressBar.Predict(quality / (_bluePrint.quality * 1.0f));

            cardsUsageLabel.text = $"{WorkBenchManager.main.allOccupiedInThisTurn}/{WorkBenchManager.main.maxWorkBenchOccupied}";
        }

        [Button]
        public void DidForgeThisTurn()
        {
            WorkBenchManager.main.DidForgeThisTurn();
        }

        public void DidPinTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.DidPinTool(location, toolOnTable);
        }
        
        public void DidUnPinTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.DidUnPinTool(location, toolOnTable);
        }
        
        public void DidPreviewTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.DidPreviewTool(location, toolOnTable);
        }
        
        public void DidUnPreviewTool(Vector2Int location, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.DidUnPreviewTool(location, toolOnTable);
        }
        
        public bool CanBePlaced(ToolOnTable toolOnTable)
        {
            return WorkBenchManager.main.CanBePlaced(toolOnTable);
        }
    }
}