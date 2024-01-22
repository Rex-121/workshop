using System.Linq;
using Tyrant.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tyrant
{
    public class FirstScene : MonoBehaviour
    {
        
        public RandomHeroStands heroStands;

        public LoadingScenes loadingScenesPrefab;

        public void SaveAndStart()
        {

            var heroes = heroStands.heroStandsArray.Select(v => v.hero);

            var squad = new HeroSquad(heroes);
            
            Storage.main.SaveSquads(new [] { squad });
            
            var loadingScenes = Instantiate(loadingScenesPrefab);
            loadingScenes.scene = LoadingScenes.Scene.SampleScene;
            
            
            gameObject.SetActive(false);
        }
        
    }
}
