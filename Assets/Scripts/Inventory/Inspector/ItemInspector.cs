using System;
using UnityEngine;

namespace Tyrant.UI
{
    public class ItemInspector: MonoBehaviour
    {
        private void Awake()
        {
            var canvasGroup = gameObject.AddComponent<CanvasGroup>();

            canvasGroup.blocksRaycasts = false;
        }

        public virtual void NewItem(IItem item)
        {
            
        }

    }
}