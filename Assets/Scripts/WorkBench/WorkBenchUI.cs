using UniRx;
using Sirenix.OdinInspector;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant.UI
{
    public class WorkBenchUI: MonoBehaviour, IWorkBenchUIHandler
    {

        public GameObject cellOnWorkBenchPrefab;

        public Transform tablePanel;

        private static WorkBench workBench => WorkBenchManager.main.workBench;
        
        public TextMeshProUGUI makeText;
        public TextMeshProUGUI qualityText;

        private void Start()
        {
            DisplayInformation();
          
            
            var list = workBench.Start();

            GetComponent<GridLayoutGroup>().constraintCount = 3;
            
            for (var i = 0; i < list.Count; i++)
            {
                var slot = list[i];
                var gb = Instantiate(cellOnWorkBenchPrefab, tablePanel);
                var cell = gb.GetComponent<SlotOnWorkBench>();
                cell.handler = this;
                cell.SetCellPosition(slot);
            }
        }

        // 显示信息
        private void DisplayInformation()
        {
            WorkBenchManager.main.make
                .Select(v => $"+ {v}")
                .Subscribe(v =>
            {
                makeText.text = v;
            }).AddTo(this);
            WorkBenchManager.main.quality.Select(v => $"+ {v}")
                .Subscribe(v =>
            {
                qualityText.text = v;
            }).AddTo(this);
        }

        [Button]
        public void DidForgeThisTurn()
        {
            WorkBenchManager.main.DidForgeThisTurn();
        }

        public void DidPinTool(Vector2Int index, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.Pin(index, toolOnTable);
        }
        
        public void DidUnPinTool(Vector2Int index, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.UnPin(index, toolOnTable);
        }
        
        public void DidPreviewTool(Vector2Int index, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.PreviewTool(toolOnTable, index);
        }
        
        public void DidUnPreviewTool(Vector2Int index, ToolOnTable toolOnTable)
        {
            WorkBenchManager.main.UnPreviewTool(index, toolOnTable);
        }
        
        public bool CanBePlaced(ToolOnTable toolOnTable)
        {
            return WorkBenchManager.main.workBench.CanBePlaced(toolOnTable);
        }
    }
}