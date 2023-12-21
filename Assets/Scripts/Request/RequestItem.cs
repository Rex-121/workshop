using System;
using Sirenix.OdinInspector;
using Tyrant;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class RequestItem : MonoBehaviour
    {

        [ShowInInspector]
        private BluePrint _bluePrint;

        public Image itemDisplay;

        public BluePrint bluePrint
        {
            set
            {
                _bluePrint = value;
                Refresh();
            }
            get => _bluePrint;
        }
        private void Refresh()
        {
            itemDisplay.sprite = bluePrint.icon;
        }

        public void TryForgeThisOne()
        {
            RequestManager.main.TryForgeThisBluePrint(bluePrint);
        }
        
    }
}
