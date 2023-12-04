using UnityEngine;
using Tools;
using UnityEngine.UI;

namespace Tyrant.UI
{
    public class ToolsBoxInCanvas: MonoBehaviour
    {

        public Transform panel;

        public GameObject toolPrefab;

        public HorizontalLayoutGroup horizontalLayoutGroup;
        public void NewTool(int index, Tool tool)
        {
            
            var gb = Instantiate(toolPrefab, panel);

            var table = gb.GetComponent<ToolOnTable>();
            table.NewTool(index, tool);
        }
        
        
    }
}