using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Tyrant
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager main;

        [ShowInInspector, NonSerialized]
        public NormalSettings normalSettings;

        private const string NormalSettingsKey = "NORMALSETTINGSKEY";

        private void Awake()
        {
            if (main == null)
            {
                main = this;

                try
                {
                    normalSettings = Storage.main.Load<NormalSettings>(NormalSettingsKey);
                }
                catch (Exception _)
                {
                    normalSettings = new NormalSettings();
                    SaveNormalSettings();
                }
            }
            else
            {
                Destroy(this);
            }
        }


        public void Save()
        {
           SaveSquads();
           
           SaveNormalSettings();
        }

        public void SaveSquads()
        {
            var squads = SquadManager.main.GetAllSquads().ToArray();
            Storage.main.SaveSquads(squads);
        }

        public HeroSquad[] LoadSquads()
        {
            var data = Storage.main.Load<HeroSquad[]>("SQUAD");
            data.ForEach(v => v.Restore());
            return data;
        }

        // 存设定
        private void SaveNormalSettings()
        { 
            Storage.main.Save(NormalSettingsKey, normalSettings);   
        }


        public void DoDeliverBirthPack()
        {
            normalSettings.birthPackDelivered = true;
            SaveNormalSettings();
        }
        
        [Serializable]
        public class NormalSettings
        {

            public bool birthPackDelivered;

        }
    }
}
