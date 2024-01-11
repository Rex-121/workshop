using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class SquadInventory: BaseDoCollect
    {
        [ShowInInspector]
        public List<IItem> inventory = new();

        public int maxSlot = 6;
        
        public IObservable<List<IItem>> items
        {
            get
            {
                _items.OnNext(inventory);
                return _items;   
            }   
        }

        private readonly ReplaySubject<List<IItem>> _items;

        public SquadInventory()
        {
            _items = new ReplaySubject<List<IItem>>(1);
            _items.OnNext(inventory);
        }

        public override void CollectItem(IItem item)
        {
            base.CollectItem(item);
            inventory.Add(item);
            _items.OnNext(inventory);
        }
    }
}