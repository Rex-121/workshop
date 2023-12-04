
using System;
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

        [ShowInInspector, NonSerialized] public WorkBench workBench = new WorkBench();
        
        public TextMeshProUGUI makeText;
        public TextMeshProUGUI qualityText;
        private void Start()
        {

            workBench.make.Select(v => $"+ {v}").Subscribe(v =>
            {
                makeText.text = v;
            });
            workBench.quality.Select(v => $"+ {v}").Subscribe(v =>
            {
                qualityText.text = v;
            });
            
            var list = workBench.Start();

            GetComponent<GridLayoutGroup>().constraintCount = 3;
            
            for (var i = 0; i < list.Count; i++)
            {
                // var b = a[i];
                // for (var j = 0; j < b.Length; j++)
                // {
                var position = list[i];
                    var gb = Instantiate(cellOnWorkBenchPrefab, tablePanel);
                    var cell = gb.GetComponent<CellOnWorkBench>();
                    cell.handler = this;
                    cell.SetCellPosition(position.position, position.type);
                // }
            }
            
            Debug.Log(transform.childCount);
            
        }

        public void DidPinTool(Vector2Int index, Tool tool)
        {
            workBench.DidPinTool(index, tool);
        }

        public void DidUnPinTool(Vector2Int index, Tool tool)
        {
            workBench.DidUnPinTool(index, tool);
        }

        public bool CanBePlaced(ToolOnTable toolOnTable)
        {
            return workBench.CanBePlaced(toolOnTable);
        }
    }
}