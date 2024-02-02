using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class CardPlacementCanvasMono: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IBeginDragHandler
    {
        public Vector3 originPosition;

        public int indexOnDeck;

        public CardEventMessageChannelSO cardEventMessageChannelSO;

        public RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void StoreIndex()
        {
            _siblingIndex = transform.GetSiblingIndex();
        }
        public void SetIndex(int index, Vector3 position)
        {
            indexOnDeck = index;

            originPosition = position;
            
            transform.SetAsFirstSibling();

            name = $"{index}-{transform.GetSiblingIndex()}";
        }

        public void DoAnimation(int zRotation, bool snap = false)
        {
            _zRotation = zRotation;
            rectTransform.DOAnchorPos(originPosition, 0.5f)
                .SetEase(Ease.OutCubic)
                .SetDelay(snap ? 0.1f : 0.3f * indexOnDeck).OnComplete(() => _isOnBoard = true);
            rectTransform.DORotate(new Vector3(0, 0, zRotation), 0.5f)
                .SetEase(Ease.OutCubic)
                .SetDelay(snap ? 0.1f : 0.3f * indexOnDeck);
        }



        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isOnBoard) return;
            if (_isExitAnimationPlaying) return;
            if (_isLock)return;
            _siblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            rectTransform.DOAnchorPos(new Vector3(0, 30, 0), 0.2f)
                .SetRelative(true);
            DoRotationAnimation(0, 0.2f);
            
            rectTransform.DOScale(new Vector3(0.2f, 0.2f, 0), 0.2f)
                .SetRelative(true);
            
            cardEventMessageChannelSO.DidSelected(this);
        }

        private void OnEnable()
        {
            cardEventMessageChannelSO.didSelected += DidSelected;
            cardEventMessageChannelSO.outSelected += OutSelected;
        }
        private void OutSelected(CardPlacementCanvasMono arg0)
        {
            if (arg0 == this) return;
            if (this == null) return;
            rectTransform.DOAnchorPos(originPosition, 0.2f);
        }
        
        private void OnDisable()
        {
            cardEventMessageChannelSO.didSelected -= DidSelected;
            cardEventMessageChannelSO.outSelected -= OutSelected;
        }
        private void DidSelected(CardPlacementCanvasMono arg0)
        {
            if (arg0 == this) return;
            var d = originPosition + new Vector3((indexOnDeck - arg0.indexOnDeck) * - 5, 0, 0);
            rectTransform.DOAnchorPos(d, 0.2f);
        }
        
        

        private int _siblingIndex = -1;
        // 发牌是否完成
        private bool _isOnBoard = false;
        private bool _isExitAnimationPlaying = false;
        private int _zRotation;
        private bool _isLock;
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isOnBoard) return;
            
            if (_isLock)return;
            
            transform.SetSiblingIndex(_siblingIndex);
            _isExitAnimationPlaying = true;
            rectTransform.DOAnchorPos(originPosition, 0.2f)
                .OnComplete(() => _isExitAnimationPlaying = false);
            DoRotationAnimation(_zRotation, 0.2f);
            rectTransform.DOScale(new Vector3(1, 1, 0), 0.2f);
            cardEventMessageChannelSO.OutSelected(this);
        }

        private void DoRotationAnimation(int rotation, float duration) => 
            rectTransform.DORotate(new Vector3(0, 0, rotation), duration)
                .SetEase(Ease.OutCubic);

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isLock)
            {
                Observable.Timer(TimeSpan.FromSeconds(0.2)).Subscribe(v =>
                {
                    _isLock = false;

                    if (eventData.pointerEnter != gameObject)
                    {
                        OnPointerExit(eventData);
                    }
                    
                }).AddTo(this);
                
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isLock = true;
        }
    }
}