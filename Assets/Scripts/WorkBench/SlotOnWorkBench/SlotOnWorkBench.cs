using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tyrant.UI
{
    // 主要空格
    // 包含 预览 放置和 buff显示
    public class SlotOnWorkBench : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        private Vector2Int _cellPosition;
        
        
        [ShowInInspector, BoxGroup("Slot"), HideLabel, HideReferenceObjectPicker]
        private WorkBenchSlot _slot;
        

        #region UIProperties
        [BoxGroup("UI")]
        public TextMeshProUGUI positionLabel;
        [BoxGroup("UI")]
        public Image backgroundImage;
        [BoxGroup("UI")]
        public Sprite a;
        [BoxGroup("UI")]
        public Sprite b;
        #endregion
        
        [BoxGroup("SlotInfo")]
        public IWorkBenchUIHandler handler;
        [BoxGroup("SlotInfo")]
        public SlotOnWorkBenchPined pined;
        [BoxGroup("SlotInfo")]
        public SlotOnWorkBenchPreview preview;
        [BoxGroup("SlotInfo")]
        public SlotOnWorkBenchBuffDisplay buffDisplay;

        private WorkBench.SlotType cellType => _slot.toolWrapper.type;
        
        [LabelText("是否可以放置骰子")]
        private bool canBePin => cellType != WorkBench.SlotType.Empty;
        public void SetCellPosition(WorkBenchSlot slot)
        {
            _slot = slot;
            
            _cellPosition = slot.toolWrapper.position;
            
            preview.RegisterSlot(slot);
            
            buffDisplay.RegisterSlot(slot);
            
            pined.RegisterSlot(slot);
            pined.cellPosition = _cellPosition;
            pined.handler = handler;
#if UNITY_EDITOR

            name = _cellPosition.ToString();

            positionLabel.text = name;
#endif
            
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
            
            
            handler?.DidPinTool(_cellPosition, toolOnTable);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!canBePin) return;
            
            var obj = eventData.pointerDrag;
            
            if (ReferenceEquals(obj, null)) return;

            var has = obj.TryGetComponent(out ToolOnTable toolOnTable);
            
            if (!has) return;
            
            // 预览
            handler?.DidPreviewTool(_cellPosition, toolOnTable);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!canBePin) return;
            
            handler?.DidUnPreviewTool(_cellPosition);
        }
    }
}
