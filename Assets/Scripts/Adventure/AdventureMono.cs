using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tyrant;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdventureMono : MonoBehaviour
{
    
    public HeroSquad[] heroSquads;

    public EnemySO enemySO;
    
    // [NonSerialized]
    // private EnemyMono _enemyMono;

    // [NonSerialized]
    // private List<IAmHero> allBattles = new();
    
    [Button]
    public void XStart()
    {

       

        for (var i = 0; i < heroSquads.Count(); i++)
        {
            var heroSquad = heroSquads[i];
            // 怪物被击杀
            heroSquad.enemyDefeated += EnemyDidDefeated;
            
            var _enemyMono = Instantiate(enemySO.enemyMonoPrefab, new Vector3(5.2f, 0, 0), Quaternion.identity, heroSquad.transform);

            _enemyMono.NewEnemy(enemySO);
            
            heroSquad.NewEnemy(_enemyMono);
        }
        
        
        // allBattles.AddRange(heroSquad.heroes);
        // allBattles.Add(_enemyMono);

    }


    private void EnemyDidDefeated(EnemyMono eMono)
    {
        Destroy(eMono.gameObject);
    }
    
    
   
}
