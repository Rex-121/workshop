using System;
using Dicing;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant.UI
{
    public class SlotOnWorkBenchPined: MonoBehaviour
    {
        public Vector2Int cellPosition;

        private IDicing _dicing;

        public TextMeshProUGUI powerDisplay;

        public IWorkBenchUIHandler handler;
        private WorkBenchSlot _slot;

        private bool _calculateUpdate = false;
        public void RegisterSlot(WorkBenchSlot slot)
        {
            _slot = slot;
            
            slot.pined
                .Subscribe(tool =>
                {
                    if (tool != null)
                    {
                        PinTool(tool);
                    }
                    else
                    {
                        DidEndThisTurn();
                    }
                })
                .AddTo(this);
        }

        private void Update()
        {
            if (!_calculateUpdate) return;
            
            powerDisplay.text = _slot.AllEffect(_dicing.Roll()).ToString();
        }

        private void PinTool(GameObject obj)
        {
            var has = obj.TryGetComponent(out ToolOnTable toolOnTable);
            
            if (!has)
            {
                gameObject.SetActive(false);
                return;
            }

            _calculateUpdate = true;
            
            toolOnTable.Lock();
            obj.transform.SetParent(transform);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rect.SetAsFirstSibling();
            var tool = obj.GetComponent<ToolOnTable>().tool;
            
            _dicing = tool.dice;

            var value = _dicing.Roll();
            
            powerDisplay.text = _slot.AllEffect(value).ToString();
            
            gameObject.SetActive(true);

            toolOnTable.startDrag = () =>
            {
                UnPinTool(toolOnTable);
            };
        }

        private void DidEndThisTurn()
        {
            gameObject.SetActive(false);
        }

        private void UnPinTool(ToolOnTable toolOnTable)
        {
            _calculateUpdate = false;
            toolOnTable.startDrag = null;
            gameObject.SetActive(false);
            handler?.DidUnPinTool(cellPosition, toolOnTable);
        }
    }
}