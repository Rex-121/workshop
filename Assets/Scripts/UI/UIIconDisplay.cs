using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class UIIconDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        public GameObject infoDisplay;

        public KeyCode keyName;

        public UIManager manager;
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            infoDisplay.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            infoDisplay.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            manager.DisplayBy(keyName);
        }
    }
}
