using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdventureMono : MonoBehaviour
{
    
    public HeroSquad[] heroSquads;

    public DungeonSO dungeonSO;

    public Vector2[] positions;

    public HeroSquad heroSquadPrefab;
    
    [Button, DisableInEditorMode]
    public void NewDungeon()
    {

        if (heroSquads.IsNullOrEmpty())
        {
            GenerateHeroSquad();
        }
        
        
        foreach (var heroSquad in heroSquads)
        {
            var dungeon = dungeonSO.toDungeon;

            // 怪物被击杀
            heroSquad.enemyDefeated += EnemyDidDefeated;
            
            // 开始`dungeon`
            heroSquad.DidEnterDungeon(dungeon);
        }
    }

    private void GenerateHeroSquad()
    {
        heroSquads = new HeroSquad[positions.Length];
        
        for (var i = 0; i < positions.Length; i++)
        {
            var position = positions[i];
            
            var heroSquad = Instantiate(heroSquadPrefab, position, Quaternion.identity, transform);

            heroSquad.transform.localPosition = position;
        
            // 怪物被击杀
            heroSquad.enemyDefeated += EnemyDidDefeated;

            heroSquads[i] = heroSquad;
        }
        
    }

    private void EnemyDidDefeated(EnemyMono eMono)
    {
        Destroy(eMono.gameObject);
    }

}
