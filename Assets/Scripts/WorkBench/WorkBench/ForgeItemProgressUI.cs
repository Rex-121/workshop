using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class ForgeItemProgressUI: MonoBehaviour
    {

        public TextMeshProUGUI makeLabel;
        public TextMeshProUGUI makeTotal;
        public TextMeshProUGUI qualityLabel;
        public TextMeshProUGUI qualityTotal;
        
        public Image makeImage;
        public Image qualityImage;
        private static ForgeItem forgeItem => WorkBenchManager.main.forgeItem;
        private void Start()
        {
            forgeItem.makeStream.Pairwise().Subscribe(SetMakeLabel).AddTo(this);
            
            forgeItem.qualityStream.Pairwise().Subscribe(SetQualityLabel).AddTo(this);
            
            makeTotal.text = $"/{forgeItem.bluePrint.make}";
            qualityTotal.text = $"/{forgeItem.bluePrint.quality}";
        }

        private void SetMakeLabel(Pair<int> value)
        {
            makeLabel.DOCounter(value.Previous, value.Current, 0.2f);
            makeImage.DOFillAmount(forgeItem.makePercent, 0.2f);
        }
        
        private void SetQualityLabel(Pair<int> value)
        {
            qualityLabel.DOCounter(value.Previous, value.Current, 0.2f);
            qualityImage.DOFillAmount(forgeItem.qualityPercent, 0.2f);
        }
    }
}