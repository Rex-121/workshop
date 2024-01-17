using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WorkBench;

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
        
        public WorkBenchEventSO workBenchEventSO;

        private void OnEnable()
        {
            workBenchEventSO.newTurnDidStarted += NewTurnDidStarted;
        }

        private void OnDisable()
        {
            workBenchEventSO.newTurnDidStarted -= NewTurnDidStarted;
        }
        
        
        private void NewTurnDidStarted(int arg0)
        {
            buffDisplay.NewTurnDidStarted(arg0);
            pined.NewTurnDidStarted(arg0);
            preview.NewTurnDidStarted(arg0);
        }

        public void SetCellPosition(WorkBenchSlot slot)
        {
            _slot = slot;

            if (cellType == WorkBench.SlotType.Empty)
            {
                buffDisplay.gameObject.SetActive(false);
                return;
            }
            
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

            if (_slot.isOccupied)
            {

                var g = _slot.pined.Value.GetComponent<ToolOnTable>();
                
                handler?.DidUnPinTool(_cellPosition, g);

                if (toolOnTable.isOnWorkBench)
                {
                    handler?.DidPinTool(toolOnTable.toolWrapper.position, g);
                }
                else
                {
                    g.BackToToolBox();
                }
                
                handler?.DidPinTool(_cellPosition, toolOnTable);
                
            }
            else
            {
                handler?.DidPinTool(_cellPosition, toolOnTable);
            }
            
            
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
            // 这里可能由于手速过快，此时已经丢失pointerDrag，导致无法获取toolOnTable
            // 所以发现鼠标移开，就强行移除
            handler?.DidUnPreviewTool(_cellPosition, null);
        }
    }
}
