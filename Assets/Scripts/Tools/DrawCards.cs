using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using Tyrant;
using UniRx;
using UnityEngine;
using WorkBench;

public class DrawCards : MonoBehaviour, ICardDeckMonoBehavior
{
    public static DrawCards main;
    
    public CardPlacementCanvasMono cardsCanvasPrefab;

    public RectTransform panel;
    
    public int[] zRotation;
    
    public Transform startPointCanvas;
    

    public Canvas canvas;
    
    public List<CardPlacementCanvasMono> allCards = new();

    public CardEventMessageChannelSO messageChannelSO;
    public WorkBenchEventSO workBenchEventSO;
    [ShowInInspector]
    public CardDeck cardDeck;
    private Vector3[] GetCurve(int count) =>  WorkBenchManager.main.curveForCard.GetCurve(count).Reverse().ToArray();

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            cardDeck = new CardDeck(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        messageChannelSO.onUse += OnUse;
        workBenchEventSO.newTurnDidStarted += NewTurnDidStarted;
    }

    private void NewTurnDidStarted(int turn)
    {
        if (turn == 1)
        {
            StackGenesisCards();
        }
        else
        {
            DrawCardsWithAnimation(Protagonist.main.drawPerTurn)
                .Subscribe()
                .AddTo(this);
        }
    }

    private void OnDisable()
    {
        messageChannelSO.onUse -= OnUse;
        workBenchEventSO.newTurnDidStarted -= NewTurnDidStarted;
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
        var tools0 = new List<CardInfoMono>();
        
        for (var i = 0; i < Protagonist.main.genesisCardAmount; i++)
        {
            tools0.Add(cardDeck.Draw());
        }

        var tools = tools0.Where(v => v != null)
            .Select(v => v.GetComponent<CardPlacementCanvasMono>())
            .ToList();
        
        var count = tools.Count();
        
        var spots = GetCurve(count);
        
        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();
        
        for (var i = 0; i < tools.Count; i ++)
        {
            var card1 = tools[i];
            card1.transform.position = startPointCanvas.position;
            card1.SetIndex(i, Camera.main.GetCanvasPosition(spots[i], canvas));
            card1.DoAnimation(c[i]);
            allCards.Add(card1);
        }
        
        tools.ForEach(v => v.StoreIndex());

    }
    
    private RectTransform location => canvas.GetComponent<RectTransform>();


    
    public IObservable<CardInfoMono> DrawCardsWithAnimation(int amount)
    {
        var l = new List<IObservable<CardInfoMono>>();
        for (var i = 0; i < amount; i++)
        {
            var tool = cardDeck.Draw();
            
            if (ReferenceEquals(tool, null)) break;
            
            l.Add(Observable.FromCoroutine(() => DrawWithAnimation(tool)).Select(_ => tool));
        }
        
        
        return Observable.Create<CardInfoMono>(v =>
        {
            l.Concat()
                .Subscribe(v.OnNext, onCompleted: v.OnCompleted)
                .AddTo(this);
            return Disposable.Empty;
        }).TakeUntilDestroy(this);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DrawWithAnimation(CardInfoMono tool)
    {
        var theCount = allCards.Count() + 1;

        var spots = GetCurve(theCount);
        
        var c = zRotation.ToList().GetRange((10 - theCount) / 2, theCount).ToArray().Reverse().ToArray();
        
        for (int i = 0; i < allCards.Count(); i ++)
        {
            var card = allCards[i];
            card.SetIndex(i, Camera.main.GetCanvasPosition(location, spots[i], canvas));
            card.DoAnimation(c[i], true);
        }
        
        var last = tool.placementCanvasMono;
        
        last.transform.position = startPointCanvas.position;
        
        last.SetIndex(theCount - 1, Camera.main.GetCanvasPosition(location, spots.Last(), canvas));
        last.DoAnimation(c.Last(), true);
        
        allCards.Add(last);
        
        allCards.ForEach(v => v.StoreIndex());

        yield return new WaitForSeconds(0.4f);

        yield return tool;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    [Button]
    public CardInfoMono DrawACard()
    {
        
        var tool = cardDeck.Draw();

        if (ReferenceEquals(tool, null)) return null;

        var theCount = allCards.Count() + 1;

        var spots = GetCurve(theCount);
        
        var c = zRotation.ToList().GetRange((10 - theCount) / 2, theCount).ToArray().Reverse().ToArray();
        
        for (int i = 0; i < allCards.Count(); i ++)
        {
            var card = allCards[i];
            card.SetIndex(i, Camera.main.GetCanvasPosition(location, spots[i], canvas));
            card.DoAnimation(c[i], true);
        }
        
        var last = tool.placementCanvasMono;
        
        last.transform.position = startPointCanvas.position;
        
        last.SetIndex(theCount - 1, Camera.main.GetCanvasPosition(location, spots.Last(), canvas));
        last.DoAnimation(c.Last(), true);
        
        allCards.Add(last);
        
        allCards.ForEach(v => v.StoreIndex());

        return last.GetComponent<CardInfoMono>();
    }

    public CardInfoMono GetCardInfoMono()
    {
        return Instantiate(cardsCanvasPrefab, startPointCanvas.position, Quaternion.identity, panel).GetComponent<CardInfoMono>();
    }
}
