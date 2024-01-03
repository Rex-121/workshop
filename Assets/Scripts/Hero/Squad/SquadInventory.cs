using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class SquadInventory
    {
        [ShowInInspector]
        public List<IItem> inventory = new();


        public void AddItem(IItem item)
        {
            inventory.Add(item);
        }
    }
}