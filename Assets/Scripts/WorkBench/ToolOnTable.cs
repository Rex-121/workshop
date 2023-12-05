using System;
using Dicing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Tools;
using UnityEngine.UI;

namespace Tyrant.UI
{
    [RequireComponent(typeof(DragInCanvas))]
    public class ToolOnTable : MonoBehaviour
    {
        
        private DragInCanvas _dragInCanvas;

        public Tool tool;

        public Image dicedImage;

        public DiceSpriteDefineSO diceSpriteDefineSO;

        private int index;
        
        public Action endDrag;
        
        public Action startDrag;
        private void Awake()
        {
            _dragInCanvas = GetComponent<DragInCanvas>();

            _dragInCanvas.endDrag += DidEndDrag;
            _dragInCanvas.startDrag += DidStartDrag;
        }

        public void NewTool(int index, Tool tool)
        {
            this.tool = tool;

            this.index = index;
            
            this.tool.LockDice();

            var value = this.tool.dice.Roll();

            dicedImage.sprite = diceSpriteDefineSO.sprites[value];
        }

        private void DidEndDrag()
        {
            endDrag?.Invoke();
            
            WorkBenchManager.main.Drag(null);
            
            if (transform.parent.TryGetComponent(out CellDragLayer cellDragLayer))
            {
                transform.SetParent(cellDragLayer.toolsBoxInCanvas.transform); 
                transform.SetSiblingIndex(index);
                transform.localScale = Vector3.one;
            }
        }
        
        private void DidStartDrag()
        {
            startDrag?.Invoke();
            transform.SetParent(ToolsBox.main.dragLayer);
            
            WorkBenchManager.main.Drag(this);
        }

        public void Lock()
        {
            _dragInCanvas.Lock();
        }
        
    }
}
