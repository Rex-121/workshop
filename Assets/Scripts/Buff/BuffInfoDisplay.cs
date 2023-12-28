using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class BuffInfoDisplay: MonoBehaviour
    {

        public BuffInfo buffInfo;

        public Image icon;
        public TextMeshProUGUI stackLabel;
        
        public void NewBuff(BuffInfo buffInfo)
        {
            this.buffInfo = buffInfo;
            icon.sprite = this.buffInfo.icon;
        }

        private void Update()
        {
            stackLabel.enabled = buffInfo.currentStack != 0;
            stackLabel.text = buffInfo.currentStackDisplay;
        }
    }
}