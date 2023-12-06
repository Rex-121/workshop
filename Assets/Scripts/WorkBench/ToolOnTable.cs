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
        private Transform _dragLayer;

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

        public void NewTool(int index, Tool tool, Transform dragLayer)
        {
            this.tool = tool;

            _dragLayer = dragLayer;

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


        public void DidUsedThisTurn()
        {
            Destroy(gameObject);
        }
        
        private void DidStartDrag()
        {
            startDrag?.Invoke();
            transform.SetParent(_dragLayer);
            
            WorkBenchManager.main.Drag(this);
        }

        public void Lock()
        {
            _dragInCanvas.Lock();
        }
        
    }
}
