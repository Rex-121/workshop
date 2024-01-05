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


        public T Load<T>(string key)
        {
            return ES3.Load<T>(key);
        }

        public void SaveSquad(HeroSquad[] squads)
        {
            Save("SQUAD", squads);
        }

        public void Save<T>(string key, T value) 
        {
            Debug.Log($"#SAVE# 存盘{key}");
            ES3.Save(key, value);
        }
        
        
    }
}