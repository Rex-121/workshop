using Tools;
using UnityEngine;

namespace Tyrant.UI
{
    public interface IWorkBenchUIHandler
    {
        // 是否可以放置
        public bool CanBePlaced(ToolOnTable toolOnTable);

        public void DidPreviewTool(Vector2Int index, ToolOnTable toolOnTable);
        
        public void DidPinTool(Vector2Int index, ToolOnTable toolOnTable);

        // public void Swap();

        public void DidUnPinTool(Vector2Int index, ToolOnTable toolOnTable);

        public void DidUnPreviewTool(Vector2Int index, ToolOnTable toolOnTable);
    }
}