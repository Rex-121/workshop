using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant.UI
{
    public class ToolsBoxInCanvas: MonoBehaviour
    {

        public Transform panel;

        public GameObject toolPrefab;

        public HorizontalLayoutGroup horizontalLayoutGroup;

        public Transform dragLayer;


        [LabelText("主Canvas"), InfoBox("主要用于获取Canvas的缩放比例")]
        public Canvas mainCanvas;
        public void NewTool(int index, Tool tool)
        {
            
            var gb = Instantiate(toolPrefab, panel);

            var table = gb.GetComponent<ToolOnTable>();
            
            table.MainCanvas(mainCanvas);
            
            table.NewTool(index, tool, dragLayer);
        }
        
        
    }
}