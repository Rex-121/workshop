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

        // public TextMeshProUGUI powerDisplay;

        public Image backgroundImage;

        public Sprite a;
        
        public Sprite b;

        public CellOnWorkBenchPined pined;
        public CellOnWorkBenchPreview preview;

        public WorkBench.SlotType cellType;

        public bool canBePin => cellType != 0;
        public void SetCellPosition(Vector2Int value, WorkBench.SlotType type)
        {
            _cellPosition = value;
#if UNITY_EDITOR

            name = _cellPosition.ToString();

            positionLabel.text = name;
#endif

            cellType = type;
            
            backgroundImage.sprite = type switch
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

        private void ResetTool(GameObject obj, ToolOnTable toolOnTable)
        {
            
            preview.UnPreviewTool();
            
            handler?.DidPinTool(_cellPosition, toolOnTable.tool);
            
            pined.PinTool(obj);

            toolOnTable.startDrag += () =>
            {
                handler?.DidUnPinTool(_cellPosition, toolOnTable.tool);
                pined.UnPinTool();
            };
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
            
            ResetTool(obj, toolOnTable);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!canBePin) return;
            
            var obj = eventData.pointerDrag;
            
            if (ReferenceEquals(obj, null)) return;

            var has = obj.TryGetComponent(out ToolOnTable toolOnTable);
            
            if (!has) return;
            
            preview.PreviewTool(toolOnTable.tool, Instantiate(obj));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!canBePin) return;
            preview.UnPreviewTool();
        }
    }
}
