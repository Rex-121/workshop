using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WorkBench
{
    public class WorkBenchMakeProgressBar: MonoBehaviour
    {

        public Image predictBar;

        public Image makeBar;

        public Ease ease = Ease.InOutCubic;

        public void Predict(float predict)
        {
            predictBar
                .DOFillAmount(predict, 0.5f)
                .SetEase(ease)
                .SetAutoKill();
        }
    }
}