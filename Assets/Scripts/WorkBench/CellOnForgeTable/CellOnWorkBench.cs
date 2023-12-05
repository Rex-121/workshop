using System;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tyrant.UI
{
    public class CellOnWorkBench : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        private Vector2Int _cellPosition;


        public TextMeshProUGUI positionLabel;

        public IWorkBenchUIHandler handler;
        
        public Image backgroundImage;

        public Sprite a;
        
        public Sprite b;

        public CellOnWorkBenchPined pined;
        public CellOnWorkBenchPreview preview;
        public CellOnWorkBenchBuffDisplay buffDisplay;
        
        public WorkBench.SlotType cellType;
        
        public bool canBePin => cellType != 0;
        public void SetCellPosition(WorkBenchSlot slot)
        {
            _cellPosition = slot.toolWrapper.position;
            
            preview.RegisterSlot(slot);
            
            buffDisplay.RegisterSlot(slot);
            
            pined.RegisterSlot(slot);
#if UNITY_EDITOR

            name = _cellPosition.ToString();

            positionLabel.text = name;
#endif

            cellType = slot.toolWrapper.type;
            
            backgroundImage.sprite = cellType switch
            {
                WorkBench.SlotType.Make => a,
                WorkBench.SlotType.Quality => b,
                _ => null
            };

            if (backgroundImage.sprite == null)
            {
                backgroundImage.enabled = false;
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (!canBePin) return;
            
            var obj = eventData.pointerDrag;

            var has = obj.TryGetComponent(out ToolOnTable toolOnTable);
            
            if (!has) return;
            
            // 是否可以放置
            var can = handler?.CanBePlaced(toolOnTable) ?? false;
            
            if (!can) return;
            
            WorkBenchManager.main.Pin(_cellPosition, toolOnTable);
            // ResetTool(obj, toolOnTable);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!canBePin) return;
            
            var obj = eventData.pointerDrag;
            
            if (ReferenceEquals(obj, null)) return;

            var has = obj.TryGetComponent(out ToolOnTable toolOnTable);
            
            if (!has) return;
            
            // 预览buff
            WorkBenchManager.main.PreviewTool(toolOnTable, _cellPosition);
            // 预览
            // preview.PreviewTool(toolOnTable, Instantiate(obj));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!canBePin) return;
            
            WorkBenchManager.main.UnPreviewTool(_cellPosition);
            // preview.UnPreviewTool();
            // buffDisplay.UnSlot();
        }
    }
}
