using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    
    /// <summary>
    /// 用于展示格子占用信息
    /// </summary>
    public class CheckerboardDisplayUI: CheckerboardBasicUI
    {
        public TextMeshProUGUI vv;


        private ReactiveProperty<int> valueD = new ReactiveProperty<int>(0);
        
        public override void SlotPrepared()
        {
            base.SlotPrepared();

            valueD
                .Pairwise()
                .Subscribe(pair => SetDisplay(pair.Previous, pair.Current))
                .AddTo(this);

            slot.scoreOnSlot
                .Subscribe(v => valueD.Value = v)
                .AddTo(this);

        }

        private void SetDisplay(int previous, int current)
        {

            if (current == 0)
            {
                vv.text = "";
            }
            else
            {
                vv.DOCounter(previous, current, 0.3f);
            }
        }

    }
}