using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using DG.Tweening;
using Sirenix.OdinInspector;
using Tyrant;
using UnityEngine;

[ExecuteInEditMode]
public class DrawCards : MonoBehaviour
{
    public CardPlacementMono cardsPrefab;


    public CurveForCard curveForCard;

    public int count = 5;

    public int[] zRotation;

    public Transform startPoint;

    public float duration = 0.5f;

    public Ease ease = Ease.OutCubic;

    public List<CardPlacementMono> allCards = new();
    
    [Button]
    public void B()
    {

        var spots = curveForCard.D(count).Reverse().ToArray();


        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();

        for (int i = 0; i < count; i ++)
        {

            var card = Instantiate(cardsPrefab);
            card.SetIndex(i);
            card.transform.position = startPoint.position;//spots[i];
            card.transform.DOMove(spots[i], duration).SetEase(ease).SetDelay(0.3f * i);
            card.transform.DORotate(new Vector3(0, 0, c[i]), duration).SetEase(ease).SetDelay(0.3f * i);
            // card.transform.eulerAngles = new Vector3(0, 0, c[i]);
            allCards.Add(card);
        }

    }
    
    [Button]
    public void C()
    {

        var theCount = allCards.Count() + 1;
        
        var spots = curveForCard.D(theCount).Reverse().ToArray();


        var c = zRotation.ToList().GetRange((10 - theCount) / 2, theCount).ToArray().Reverse().ToArray();

        
        
        for (int i = 0; i < allCards.Count(); i ++)
        {

            var card = allCards[i];
            // card.SetIndex(i);
            // card.transform.position = startPoint.position;//spots[i];
            card.transform.DOMove(spots[i], duration).SetEase(ease).SetDelay(0.1f);
            card.transform.DORotate(new Vector3(0, 0, c[i]), duration).SetEase(ease).SetDelay(0.1f);
            // card.transform.eulerAngles = new Vector3(0, 0, c[i]);
        }

        var last = Instantiate(cardsPrefab);
        
        last.SetIndex(theCount);
        last.transform.position = startPoint.position;
        
        last.transform.DOMove(spots.Last(), duration).SetEase(ease).SetDelay(0.3f * 1);
        last.transform.DORotate(new Vector3(0, 0, c.Last()), duration).SetEase(ease).SetDelay(0.3f * 1);
        
        allCards.Add(last);
    }
    
}
