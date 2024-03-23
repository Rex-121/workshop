using System;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using Tyrant;
using UniRx;
using UnityEngine;

public class DrawCards : MonoBehaviour, ICardDeckMonoBehavior
{
    public CardPlacementCanvasMono cardsCanvasPrefab;

    public RectTransform panel;

    // public CurveForCard curveForCard;
    
    public int[] zRotation;
    
    public Transform startPointCanvas;
    

    public Canvas canvas;
    
    public List<CardPlacementCanvasMono> allCards = new();

    public CardEventMessageChannelSO messageChannelSO;
    [ShowInInspector]
    public CardDeck cardDeck;
    private Vector3[] GetCurve(int count) =>  WorkBenchManager.main.curveForCard.GetCurve(count).Reverse().ToArray();

    private void Awake()
    {
        cardDeck = new CardDeck(this);
    }

    private void Start()
    {
        cardDeck.Start();
        StackGenesisCards();
    }

    private void OnEnable()
    {
        messageChannelSO.onUse += OnUse;
    }

    private void OnDisable()
    {
        messageChannelSO.onUse -= OnUse;
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


    /// <summary>
    /// 初始化牌堆
    /// </summary>
    [Button]
    public void StackGenesisCards()
    {
        var tools = new List<CardPlacementCanvasMono>();
        for (var i = 0; i < 5; i++)
        {
            tools.Add(cardDeck.Draw().GetComponent<CardPlacementCanvasMono>());
        }
        
        var count = tools.Count();
        
        var spots = GetCurve(count);
        
        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();

        for (var i = 0; i < 5; i ++)
        {
            var card1 = tools[i];
            card1.transform.position = startPointCanvas.position;
            card1.SetIndex(i, Camera.main.GetCanvasPosition(spots[i], canvas));
            card1.DoAnimation(c[i]);
            allCards.Add(card1);
        }
        
        tools.ForEach(v => v.StoreIndex());

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

        
        var tool = cardDeck.Draw();
        // last.GetComponent<CardInfoMono>().NewTool(tool);
        var last = tool.GetComponent<CardPlacementCanvasMono>();

        
        last.transform.position = startPointCanvas.position;
        
        last.SetIndex(theCount - 1, Camera.main.GetCanvasPosition(spots.Last(), canvas));
        last.DoAnimation(c.Last(), true);
        
        allCards.Add(last);
        
        allCards.ForEach(v => v.StoreIndex());
    }

    public CardInfoMono GetCardInfoMono()
    {
        return Instantiate(cardsCanvasPrefab, startPointCanvas.position, Quaternion.identity, panel).GetComponent<CardInfoMono>();
    }
}
