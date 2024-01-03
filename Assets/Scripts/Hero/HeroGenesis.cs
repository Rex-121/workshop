using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/HeroGenesis", fileName = "HeroGenesis")]
    public class HeroGenesis: SingletonSO<HeroGenesis>
    {

        public CharacterSO[] characterSos;

        public JobSO[] jobSos;

        // public string[] heroCodes;

        public HeroSquad[] squads;

        public IEnumerable<HeroSquad> GetAllSquads()
        {

            squads = ES3.Load<HeroSquad[]>("SQUAD");
            foreach (var heroSquad in squads)
            {
                heroSquad.Restore();
            }
            return squads;
            // return new []{}
            // return heroCodes.Select(SquadByCode);
        }

        // public HeroSquad SquadByCode(string store)
        // {
        //     var squads = store.Split("-");
        //     var aTeams = squads.Select(v => v.Split(":"));
        //     var squad = aTeams.Select(v => HeroGenesis.main.RestoreByIDs(v));
        //     return new HeroSquad(squad);
        // }

        public Hero RandomHero()
        {
            var hero = Hero.FromSO(characterSos.RandomElement(), jobSos.RandomElement());
            hero.equipments.ChangeEquipment(Arsenal.main.RandomWeapon());
            return hero;
        }
        
        public Hero RestoreByIDs(string[] ids)
        {
            return Hero.FromSO
            (
                FindCharacterByID(int.Parse(ids.First())),
                FindJobByID(int.Parse(ids[1]))
            );
        }

        public CharacterSO FindCharacterByID(int id)
        {
            return Instantiate(characterSos.First(v => v.id == id));
        }
        
        public JobSO FindJobByID(int id)
        {
            return Instantiate(jobSos.First(v => v.id == id));
        }
        
#if UNITY_EDITOR
        
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateOverview()
        {
            characterSos = AssetDatabase.FindAssets("t:CharacterSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<CharacterSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            jobSos = AssetDatabase.FindAssets("t:JobSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<JobSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
            
#endif
        
    }
}