using System.Linq;
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

        public string[] heroCodes;
        
        public Hero RestoreByIDs(string[] ids)
        {
            return Hero.FromSO
            (
                FindCharacterByID(int.Parse(ids.First())),
                FindJobByID(int.Parse(ids[1]))
            );
        }

        private CharacterSO FindCharacterByID(int id)
        {
            return Instantiate(characterSos.First(v => v.id == id));
        }
        
        private JobSO FindJobByID(int id)
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