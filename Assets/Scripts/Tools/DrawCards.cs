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
    public CardPlacementMono cardsPrefab;


    public CurveForCard curveForCard;
    
    public int[] zRotation;

    public Transform startPoint;

    public float duration = 0.5f;

    public Ease ease = Ease.OutCubic;

    public List<CardPlacementMono> allCards = new();

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
        messageChannelSO.didSelected += DidSelected;
        messageChannelSO.outSelected += OutSelected;
    }

    private void OutSelected(CardPlacementMono arg0)
    {
        if (cardPlacementMono == arg0)
        {
            gb = Instantiate(arg0.dice);    
        }
    }

    private void OnDisable()
    {
        messageChannelSO.didSelected -= DidSelected;
    }

    public CardPlacementMono cardPlacementMono;
    public GameObject gb;

    private void DidSelected(CardPlacementMono arg0)
    {
        Debug.Log("jfladjsfasf");

        cardPlacementMono = arg0;
    }

    private void Update()
    {
        if (gb != null)
        {
            var v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gb.transform.position = new Vector3(v.x, v.y, 0);
        }
    }

    [Button]
    public void Draw()
    {

        var count = cardDeck.GenesisDraw().Count();
        
        var spots = GetCurve(count);
        
        var c = zRotation.ToList().GetRange((10 - count) / 2, count).ToArray().Reverse().ToArray();

        for (int i = 0; i < count; i ++)
        {
            var card = Instantiate(cardsPrefab);
            
            card.transform.position = startPoint.position;
            card.SetIndex(i, spots[i]);
            card.DoAnimation(c[i]);
            allCards.Add(card);
        }

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
            card.SetIndex(i, spots[i]);
            card.DoAnimation(c[i], true);
        }

        var last = Instantiate(cardsPrefab);
        
        last.transform.position = startPoint.position;
        
        last.SetIndex(theCount - 1, spots.Last());
        last.DoAnimation(c.Last(), true);
        
        allCards.Add(last);
    }
    
}
