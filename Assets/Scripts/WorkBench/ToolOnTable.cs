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

        public WorkBench.ToolWrapper toolWrapper;
        
        public Tool tool;

        [SerializeField]
        private DiceBuffDataSO diceBuffDataSO;

        public DiceBuffInfo diceBuffInfo;// => diceBuffDataSO.ToBuff();
        
        public Image dicedImage;

        public DiceSpriteDefineSO diceSpriteDefineSO;

        private int index;

        public bool isOnWorkBench = false;
        
        public Action endDrag;
        
        public Action startDrag;
        private void Awake()
        {
            _dragInCanvas = GetComponent<DragInCanvas>();

            _dragInCanvas.endDrag += DidEndDrag;
            _dragInCanvas.startDrag += DidStartDrag;

            diceBuffInfo = diceBuffDataSO.ToBuff();
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

        public void BackToToolBox()
        {
            DidStartDrag();
            DidEndDrag();
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
                isOnWorkBench = false;
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
            isOnWorkBench = true;
            _dragInCanvas.Lock();
        }
        
    }
}
