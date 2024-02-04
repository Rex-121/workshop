// using System;
// using DG.Tweening;
// using DG.Tweening.Core;
// using DG.Tweening.Plugins.Options;
// using UniRx;
// using UniRx.Triggers;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.Rendering;
//
// namespace Tyrant
// {
//     public class CardPlacementMono: MonoBehaviour
//     {
//
//         public Canvas canvas;
//
//
//         public SortingGroup sortingGroup;
//         
//         public GameObject dice;
//
//         public Vector3 originPosition;
//
//         private int _indexOnDeck;
//
//         public void Start()
//         {
//             // gameObject.OnMouseEnterAsObservable()
//             //     .ThrottleFrame(60)
//             //     .Subscribe(c =>
//             //     {
//             //         v();
//             //     });
//         }
//
//         public CardEventMessageChannelSO cardEventMessageChannelSO;
//
//         private int sortingOrder => 100 - _indexOnDeck;
//
//         public void SetIndex(int index, Vector3 position)
//         {
//             _indexOnDeck = index;
//
//             originPosition = position;
//
//             // sortingGroup.sortingOrder = sortingOrder;
//
//             // canvas.sortingOrder = sortingOrder;
//         }
//
//         public void DoAnimation(int zRotation, bool snap = false)
//         {
//             GetComponent<RectTransform>().DOAnchorPos(originPosition, 0.5f)
//                 .SetEase(Ease.OutCubic)
//                 .SetDelay(snap ? 0.1f : 0.3f * _indexOnDeck);
//             GetComponent<RectTransform>().DORotate(new Vector3(0, 0, zRotation), 0.5f)
//                 .SetEase(Ease.OutCubic)
//                 .SetDelay(snap ? 0.1f : 0.3f * _indexOnDeck);
//         }
//         
//
//         private void OnMouseDown()
//         {
//             Debug.Log("OnMouseDown");
//             // cardEventMessageChannelSO.DidSelected(this);
//         }
//
//         public void OnPointerEnter(PointerEventData eventData)
//         {
//             Debug.Log(sortingGroup.sortingOrder);
//         }
//
//         public bool isChecking = false;
//
//         public TweenerCore<Vector3, Vector3, VectorOptions> start;
//         
//         private void OnMouseEnter()
//         {
//             Debug.Log("OnMouseEnter");
//             sortingGroup.sortingOrder = 200;
//
//             canvas.sortingOrder = 200;
//
//             if (!isChecking)
//             {
//                 isChecking = true;
//                 start = transform.DOMove(new Vector3(0, 1, 0), 0.2f).SetEase(Ease.InCubic).SetRelative(true).OnComplete(() =>
//                 {
//                     // Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(v =>
//                     // {
//                         isChecking = false;
//                     // });
//                     
//                 });
//             }
//             
//             
//         }
//
//         // public bool c = false;
//
//         private void OnMouseExit()
//         {
//             Debug.Log("OnMouseExit");
//         
//             
//             sortingGroup.sortingOrder = sortingOrder;
//
//             canvas.sortingOrder = sortingOrder;
//             
//             // if (isChecking)return;
//             // {
//             //
//             
//             // if (c) return;
//             isChecking = true;
//             // c = true;
//             // start?.Kill();
//                 transform.DOMove(originPosition, 0.3f).OnComplete(() =>
//                 {
//                     
//                     // Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(v =>
//                     // {
//                         isChecking = false;
//                     // });
//                 });
//                 
//                 
//                 // cardEventMessageChannelSO.OutSelected(this);
//                 // transform.DOMove(new Vector3(0, 1, 0), 0.3f).SetRelative(true);
//             // }
//
//             // isChecking = false;
//
//         }
//     }
// }