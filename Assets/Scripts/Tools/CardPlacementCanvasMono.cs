using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class CardPlacementCanvasMono : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Vector3 originPosition;

        public int indexOnDeck;

        public CardEventMessageChannelSO cardEventMessageChannelSO;

        public RectTransform rectTransform;

        public CanvasGroup canvasGroup;
        

        public void StoreIndex()
        {
            _siblingIndex = transform.GetSiblingIndex();
            name = $"{indexOnDeck}-{transform.GetSiblingIndex()}";
            canvasGroup.blocksRaycasts = true;
        }

        public void SetIndex(int index, Vector3 position)
        {
            indexOnDeck = index;

            originPosition = position;

            transform.SetAsFirstSibling();
        }

        public void DoAnimation(int zRotation, bool snap = false)
        {
            _zRotation = zRotation;
            rectTransform.DOAnchorPos(originPosition, 0.5f)
                .SetEase(Ease.OutCubic)
                .SetDelay(snap ? 0.1f : 0.3f * indexOnDeck);
            rectTransform.DORotate(new Vector3(0, 0, zRotation), 0.5f)
                .SetEase(Ease.OutCubic)
                .SetDelay(snap ? 0.1f : 0.3f * indexOnDeck).OnComplete(() => enabled = true);
        }


        //public bool d;
        
        public void OnPointerEnter(PointerEventData eventData)
        {

            if (_isLock) return;
            
            
            _siblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();

                // 动画
            rectTransform.DOAnchorPos(originPosition + new Vector3(0, 30, 0), 0.2f);
            DoRotationAnimation(0, 0.2f);
            rectTransform.DOScale(new Vector3(1.2f, 1.2f, 0), 0.2f);
            

            cardEventMessageChannelSO.DidSelected(this);
        }

        private void OnEnable()
        {
            cardEventMessageChannelSO.didSelected += DidSelected;
            cardEventMessageChannelSO.outSelected += OutSelected;
            cardEventMessageChannelSO.onBeginDrag += OnBeginDrag;
            cardEventMessageChannelSO.onEndDrag += OnEndDrag;
            cardEventMessageChannelSO.onUse += OnUse;
        }
        
        private void OnDisable()
        {
            cardEventMessageChannelSO.didSelected -= DidSelected;
            cardEventMessageChannelSO.outSelected -= OutSelected;
            cardEventMessageChannelSO.onBeginDrag -= OnBeginDrag;
            cardEventMessageChannelSO.onEndDrag -= OnEndDrag;
            cardEventMessageChannelSO.onUse -= OnUse;
        }

        private void OnUse(CardPlacementCanvasMono arg0)
        {
            if (arg0 == this)
            {
                Destroy(GetComponent<CardDraggingMono>().draggingDice);
                cardEventMessageChannelSO.OutSelected(this);
            }
            else
            {
                BackToDefaultState();
            }
        }



        private void OnEndDrag(CardDraggingMono arg0)
        {
            DoEndAndClearCard();
        }

        private void OnBeginDrag(CardDraggingMono arg0)
        {
            _isLock = true;
            canvasGroup.blocksRaycasts = false;
        }

        private void OutSelected(CardPlacementCanvasMono arg0)
        {
            if (arg0 == this || this == null) return;
            // 开始非选中的卡牌归位的动画
            // rectTransform.DOAnchorPos(originPosition, 0.2f);
        }

        private void DidSelected(CardPlacementCanvasMono arg0)
        {
            // 如果选中的卡牌是自己, 则不做让位动画
            if (arg0 == this) return;
            
            // 开始非选中的卡牌让位的动画
            var d = originPosition + new Vector3((indexOnDeck - arg0.indexOnDeck) * -5, 0, 0);
            rectTransform.DOAnchorPos(d, 0.2f);
        }



        private int _siblingIndex = -1;

        private int _zRotation;

        // 是否处于选牌阶段
        [ShowInInspector]
        private bool _isLock;

        public void OnPointerExit(PointerEventData eventData)
        {

            if (_isLock) return;

            DoExitAnimation();
        }

        private void DoRotationAnimation(int rotation, float duration) =>
            rectTransform.DORotate(new Vector3(0, 0, rotation), duration)
                .SetEase(Ease.OutCubic);

        private void DoEndAndClearCard()
        {

            Observable.Timer(TimeSpan.FromSeconds(0.2)).Subscribe(v =>
            {
                if (_isLock)
                {
                    DoExitAnimation();
                }

                BackToDefaultState();
            }).AddTo(this);

        }


        private void BackToDefaultState()
        {
            canvasGroup.blocksRaycasts = true;
            _isLock = false;
        }

        private void DoExitAnimation()
        {
            transform.SetSiblingIndex(_siblingIndex);
            
            // 动画
            rectTransform.DOAnchorPos(originPosition, 0.2f);
            DoRotationAnimation(_zRotation, 0.2f);
            rectTransform.DOScale(new Vector3(1, 1, 0), 0.2f);
            
            cardEventMessageChannelSO.OutSelected(this);
        }
        
    }
}