using Dicing;
using TMPro;
using Tools;
using UnityEngine;

namespace Tyrant.UI
{
    public class CellOnWorkBenchPined: MonoBehaviour
    {
        private Vector2Int _cellPosition;

        private IDicing _dicing;

        public TextMeshProUGUI powerDisplay;

        public void PinTool(GameObject obj)
        {
            var has = obj.TryGetComponent(out ToolOnTable toolOnTable);
            
            if (!has)
            {
                gameObject.SetActive(false);
                return;
            }
            
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

            powerDisplay.text = _dicing.Roll().ToString();
            
            gameObject.SetActive(true);
        }

        public void UnPinTool()
        {
            gameObject.SetActive(false);
        }
    }
}