using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using DG.Tweening;
using Sirenix.OdinInspector;
using Tyrant;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public CardPlacementCanvasMono cardsCanvasPrefab;

    public RectTransform panel;

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

    private void OnEnable()
    {
        messageChannelSO.onUse += OnUse;
    }

    private void OnUse(CardPlacementCanvasMono arg0)
    {
        allCards.Remove(arg0);

        var count = allCards.Count;
        
        var spots = GetCurve(count);
        
        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();

        for (int i = 0; i < count; i ++)
        {
            var card1 = allCards[i];
            
            card1.SetIndex(i, Camera.main.GetCanvasPosition(spots[i], canvas));
            card1.DoAnimation(count == 1 ? 0:c[i], true);
        }
    }

    private void OnDisable()
    {
        messageChannelSO.onUse -= OnUse;
    }

    [Button]
    public void Draw()
    {

        var tools = cardDeck.GenesisDraw();
        
        var count = tools.Count();
        
        var spots = GetCurve(count);
        
        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();

        for (int i = 0; i < count; i ++)
        {
            var card1 = Instantiate(cardsCanvasPrefab, panel);
            
            card1.GetComponent<CardInfoMono>().NewTool(tools[i]);
            
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
        
        var tool = cardDeck.Draw();
        last.GetComponent<CardInfoMono>().NewTool(tool);

        
        last.transform.position = startPointCanvas.position;
        
        last.SetIndex(theCount - 1, Camera.main.GetCanvasPosition(spots.Last(), canvas));
        last.DoAnimation(c.Last(), true);
        
        allCards.Add(last);
        
        allCards.ForEach(v => v.StoreIndex());
    }
    
}
