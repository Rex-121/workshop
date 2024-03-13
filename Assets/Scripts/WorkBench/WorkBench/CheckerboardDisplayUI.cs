using System;
using TMPro;
using UnityEngine;
using UniRx;

namespace Tyrant
{
    
    /// <summary>
    /// 用于展示预览
    /// </summary>
    public class CheckerboardDisplayUI: MonoBehaviour
    {
        public TextMeshProUGUI vv;

        public void SetV(Tool tool)
        {
            vv.text = $"+ {tool.dice.Roll()}";
            
        }

        private void Start()
        {
           
            
            
        }

        public void SetBuffPreview()
        {
            
        }
        
        

    }
}