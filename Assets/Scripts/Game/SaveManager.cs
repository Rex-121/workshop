using System;
using System.Linq;
using UnityEngine;

namespace Tyrant
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager main;
        private void Awake()
        {
            if (main == null)
            {
                main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }


        public void Save()
        {
            var squads = SquadManager.main.GetAllSquads().ToArray();
            ES3.Save("SQUAD", squads);
        }
    }
}
