using UniRx;
using Sirenix.OdinInspector;
using TMPro;
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