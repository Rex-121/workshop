using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/Storage")]
    public class Storage: SingletonSO<Storage>
    {


        public Dictionary<int, Inventory.Slot> LoadAllItems(string key)
        {
            try
            {
                return ES3.Load<Dictionary<int, Inventory.Slot>>(key);
            }
            catch (Exception e)
            {
                return new Dictionary<int, Inventory.Slot> { };
            }
        }

        public void Save<T>(string key, T value) 
        {
            ES3.Save(key, value);
        }
        
        
    }
}