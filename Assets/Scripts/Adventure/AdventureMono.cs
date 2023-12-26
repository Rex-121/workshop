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

    public DungeonSO dungeonSO;
    
    [Button, DisableInEditorMode]
    public void XStart()
    {

       

        for (var i = 0; i < heroSquads.Count(); i++)
        {
            
            var dungeon = dungeonSO.toDungeon;
            
            var heroSquad = heroSquads[i];
            // 怪物被击杀
            heroSquad.enemyDefeated += EnemyDidDefeated;
            
            // 开始`dungeon`
            heroSquad.DidEnterDungeon(dungeon);
        }
        
    }


    private void EnemyDidDefeated(EnemyMono eMono)
    {
        Destroy(eMono.gameObject);
    }

}
