using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Tyrant
{
    public class CardPlacementCanvasMono : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Vector3 originPosition;

        public int indexOnDeck;

        public CardEventMessageChannelSO cardEventMessageChannelSO;

        public RectTransform rectTransform;

        public CanvasGroup canvasGroup;
        
        // private void Awake()
        // {
        //     rectTransform = GetComponent<RectTransform>();
        // }

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
                .SetDelay(snap ? 0.1f : 0.3f * indexOnDeck);
        }


        public bool d;
        
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
        }
        
        private void OnDisable()
        {
            cardEventMessageChannelSO.didSelected -= DidSelected;
            cardEventMessageChannelSO.outSelected -= OutSelected;
            cardEventMessageChannelSO.onBeginDrag -= OnBeginDrag;
            cardEventMessageChannelSO.onEndDrag -= OnEndDrag;
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
            if (arg0 == this) return;
            if (this == null) return;
            rectTransform.DOAnchorPos(originPosition, 0.2f);
        }

        private void DidSelected(CardPlacementCanvasMono arg0)
        {
            if (arg0 == this) return;
            var d = originPosition + new Vector3((indexOnDeck - arg0.indexOnDeck) * -5, 0, 0);
            rectTransform.DOAnchorPos(d, 0.2f);
        }



        private int _siblingIndex = -1;

        private int _zRotation;

        // 是否处于选牌阶段
        private bool _isLock;
        private IPointerMoveHandler _pointerMoveHandlerImplementation;

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
                    canvasGroup.blocksRaycasts = true;
                    DoExitAnimation();
                }
                _isLock = false;
                
            }).AddTo(this);

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