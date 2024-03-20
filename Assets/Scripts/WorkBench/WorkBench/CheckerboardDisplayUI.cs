using DG.Tweening;
using Dicing;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    
    /// <summary>
    /// 用于展示格子占用信息
    /// </summary>
    public class CheckerboardDisplayUI: CheckerboardBasicUI
    {
        public TextMeshProUGUI scoreDisplay;
        
        private ReactiveProperty<int> valueD = new ReactiveProperty<int>(0);

        public DiceSpriteDefineSO diceSpriteDefineSO;
        public Image diceDisplay;
        
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

            slot.toolInSlot
                .Subscribe(SetDiceSprite)
                .AddTo(this);

        }

        private void SetDiceSprite(Tool tool)
        {
            diceDisplay.enabled = tool != null;

            diceDisplay.sprite = null;
            
            if (tool != null)
            {
                diceDisplay.sprite = diceSpriteDefineSO.sprites[tool.dice.Roll()];
            }
        }

        private void SetDisplay(int previous, int current)
        {
            
            Debug.Log($"SetDisplay {previous}, {current}");

            if (current == 0)
            {
                scoreDisplay.text = "";
            }
            else
            {
                scoreDisplay.DOCounter(previous, current, 0.3f);
            }
        }

    }
}