using System.Linq;
using Tyrant.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tyrant
{
    public class FirstScene : MonoBehaviour
    {
        
        public RandomHeroStands heroStands;


        public void SaveAndStart()
        {

            var heroes = heroStands.heroStandsArray.Select(v => v.hero);

            var squad = new HeroSquad(heroes);
            
            Storage.main.SaveSquad(new HeroSquad[] { squad });
            
            SceneManager.LoadScene("Scenes/SampleScene");

        }
        
    }
}
