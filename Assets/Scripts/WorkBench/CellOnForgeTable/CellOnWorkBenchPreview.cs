using Dicing;
using TMPro;
using Tools;
using UnityEngine;

namespace Tyrant.UI
{
    public class CellOnWorkBenchPreview: MonoBehaviour
    {
        
        private Vector2Int _cellPosition;


        public TextMeshProUGUI powerDisplay;

        private GameObject _copyDice;
        
        public void PreviewTool(Tool tool, GameObject copy)
        {
            var dicing = tool.dice;

            powerDisplay.text = dicing.Roll().ToString();

            _copyDice = copy;
            
            _copyDice.transform.SetParent(transform);
            _copyDice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            _copyDice.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            _copyDice.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            _copyDice.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
            
            gameObject.SetActive(true);
        }

        public void UnPreviewTool()
        {
            if (!ReferenceEquals(_copyDice, null))
            {
                Destroy(_copyDice);
            }
            
            gameObject.SetActive(false);
        }
    }
}