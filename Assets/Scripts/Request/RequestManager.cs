// using System;
// using System.Linq;
// using Sirenix.Utilities;
// using UnityEngine;
//
// namespace Tyrant
// {
//     public class RequestManager: MonoBehaviour
//     {
//
//         public static RequestManager main;
//         
//         private void Awake()
//         {
//             if (main == null)
//             {
//                 main = this;
//                 // DontDestroyOnLoad(this);
//             }
//             else
//             {
//                 Destroy(this);
//             }
//         }
//
//         public GameObject requestInventoryBagPrefab;
//
//         private GameObject _thisOne;
//
//         public BluePrint bluePrint;
//
//         public Transform requestPanel;
//         public RequestItem requestItemPrefab;
//         // private void Start()
//         // {
//         //     RequestGenesis.main.bluePrintSO
//         //         .Select(BluePrint.FromSO)
//         //         .ForEach(v =>
//         //     {
//         //         var requestItem = Instantiate(requestItemPrefab, requestPanel).GetComponent<RequestItem>();
//         //         requestItem.bluePrint = v;
//         //     });
//         // }
//
//         public void PreviewBluePrint(BluePrint bp, HeroMono heroMono)
//         {
//             var gb = Instantiate(requestItemPrefab, requestPanel);
//             gb.GetComponent<RequestItem>().bluePrint = bp;
//             gb.GetComponent<HeroInfoDisplay>().hero = heroMono.heroic as Hero;
//         }
//
//         public void TryForgeThisBluePrint(BluePrint bp)
//         {
//             if (_thisOne != null)
//             {
//                 Destroy(_thisOne);
//             }
//             bluePrint = bp;
//             _thisOne = Instantiate(requestInventoryBagPrefab);
//
//         }
//         
//     }
// }