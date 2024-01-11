using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class ESExample: MonoBehaviour
    {


        [ShowInInspector]
        public HeroEquipments heroEquipments;

        
        [ShowInInspector]
        public HeroEquipments heroEquipments1;

        public Attribute attribute = new Attribute();


        public Attribute attribute1;

        public CharacterSO characterSO;
        public JobSO jobSO;


        [ShowInInspector]
        public Health health;
        [ShowInInspector]
        public Health health1;
        [ShowInInspector]
        public Hero hero;
        
        [ShowInInspector]
        public Hero hero1;
        private void Start()
        {

            hero = Hero.FromSO(characterSO, jobSO);
            // hero1 = Hero.FromSO(characterSO, jobSO);
            heroEquipments = new HeroEquipments();

            health = new Health(new Attribute(5,5,5), new HeroHealthStrategy(1,1,1,1));
        }


        [Button]
        public void Save()
        {
            ES3.Save("attribute", attribute);
            ES3.Save("heroEquipments", heroEquipments);
            ES3.Save("hero", hero);
            ES3.Save("health", health);
            
            ES3.Save("Squad", new HeroSquad(new []{ hero, hero, hero }));
        }

        [Button]
        public void Load()
        {
            attribute1 = ES3.Load<Attribute>("attribute");
            heroEquipments1 = ES3.Load<HeroEquipments>("heroEquipments");
            hero1 = ES3.Load<Hero>("hero");

            health1 = ES3.Load<Health>("health");
        }
    }
}