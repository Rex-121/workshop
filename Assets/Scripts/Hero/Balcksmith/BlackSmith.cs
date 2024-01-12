using System;
using Sirenix.OdinInspector;
using UnityEngine;
using WorkBench;

namespace Tyrant.BlackSmith
{
    public class BlackSmith : MonoBehaviour
    {
        [Button]
        public void SetToHammerOrNot()
        {
            _isHammer = !_isHammer;
            PlayHammerAnimation();
        }

        private void PlayHammerAnimation()
        {
            _animator.SetBool(IsHammer, _isHammer);
        }

        public SpriteRenderer forgeItem;

        private bool _isHammer = false;

        public WorkBenchEventSO workBenchEventSO;

        private Animator _animator;
        private static readonly int IsHammer = Animator.StringToHash("isHammer");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            workBenchEventSO.prepareNewRound += StartHammer;
            workBenchEventSO.turnDidEnded += EndHammer;
            workBenchEventSO.blueprintDidSelected += BlueprintDidSelected;
            workBenchEventSO.roundDidEnded += RoundDidEnded;
        }

        private void RoundDidEnded(int arg0)
        {
            forgeItem.sprite = null;
        }

        private void BlueprintDidSelected(BluePrint arg0)
        {
            forgeItem.sprite = arg0.icon;
        }

        private void OnDisable()
        {
            workBenchEventSO.prepareNewRound -= StartHammer;
            workBenchEventSO.turnDidEnded -= EndHammer;
            workBenchEventSO.blueprintDidSelected -= BlueprintDidSelected;
            workBenchEventSO.roundDidEnded -= RoundDidEnded;
        }

        private void StartHammer(int arg0)
        {
            _isHammer = true;
            PlayHammerAnimation();
        }

        private void EndHammer(int arg0)
        {
            _isHammer = false;
            PlayHammerAnimation();
        }
    }
}
