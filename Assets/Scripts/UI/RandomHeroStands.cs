using UnityEngine;

namespace Tyrant.UI
{
    public class RandomHeroStands : MonoBehaviour
    {

        public Transform[] transforms;

        public HeroStands[] heroStandsArray;

        public HeroStands heroStandsPrefab;
        
        private void Start()
        {

            heroStandsArray = new HeroStands[transforms.Length];
            
            for (var i = 0; i < transforms.Length; i++)
            {
                heroStandsArray[i] = Instantiate(heroStandsPrefab, transforms[i]);
                heroStandsArray[i].hero = HeroGenesis.main.RandomHero();
            }
            
        }

        public void Random(int index)
        {
            heroStandsArray[index].hero = HeroGenesis.main.RandomHero();
        }
    }
}
