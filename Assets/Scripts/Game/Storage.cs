using System;
using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/Storage")]
    public class Storage: SingletonSO<Storage>
    {


        public Inventory.Slot[] LoadAllItems(string key)
        {
            try
            {
                return ES3.Load<Inventory.Slot[]>(key);
            }
            catch (Exception e)
            {
                return new Inventory.Slot[] { };
            }
        }

        public void Save<T>(string key, T value) 
        {
            ES3.Save(key, value);
        }
        
        
    }
}