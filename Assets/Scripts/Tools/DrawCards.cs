using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using DG.Tweening;
using Sirenix.OdinInspector;
using Tyrant;
using UniRx;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public CardPlacementCanvasMono cardsCanvasPrefab;

    public CurveForCard curveForCard;
    
    public int[] zRotation;
    
    public Transform startPointCanvas;
    

    public Canvas canvas;
    
    public List<CardPlacementCanvasMono> allCards = new();

    public CardEventMessageChannelSO messageChannelSO;
    [ShowInInspector]
    public CardDeck cardDeck;
    private Vector3[] GetCurve(int count) => curveForCard.GetCurve(count).Reverse().ToArray();

    private void Awake()
    {
        cardDeck = new CardDeck();
    }

    private void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(v =>
        {
            Draw();

        }).AddTo(this);
    }

    [Button]
    public void Draw()
    {

        var count = cardDeck.GenesisDraw().Count();
        
        var spots = GetCurve(count);
        
        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();

        for (int i = 0; i < count; i ++)
        {
            var card1 = Instantiate(cardsCanvasPrefab, canvas.transform);
            card1.transform.position = startPointCanvas.position;
            card1.SetIndex(i, Camera.main.GetCanvasPosition(spots[i], canvas));
            card1.DoAnimation(c[i]);
            // yield return new WaitForSeconds(0.3f);
            // card1.transform.SetAsFirstSibling();
            allCards.Add(card1);
        }
        
        allCards.ForEach(v => v.StoreIndex());

    }
    
    [Button]
    public void DrawACard()
    {

        var theCount = allCards.Count() + 1;

        var spots = GetCurve(theCount);
        
        var c = zRotation.ToList().GetRange((10 - theCount) / 2, theCount).ToArray().Reverse().ToArray();
        
        for (int i = 0; i < allCards.Count(); i ++)
        {
            var card = allCards[i];
            card.SetIndex(i, Camera.main.GetCanvasPosition(spots[i], canvas));
            card.DoAnimation(c[i], true);
        }

        var last = Instantiate(cardsCanvasPrefab, canvas.transform);
        
        last.transform.position = startPointCanvas.position;
        
        last.SetIndex(theCount - 1, Camera.main.GetCanvasPosition(spots.Last(), canvas));
        last.DoAnimation(c.Last(), true);
        
        allCards.Add(last);
        
        allCards.ForEach(v => v.StoreIndex());
    }
    
}
