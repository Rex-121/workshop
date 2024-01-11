using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tyrant;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AdventureMono : MonoBehaviour
{

    private Dictionary<int, HeroSquadMono> heroSquads = new();

    public DungeonSO dungeonSO;

    public Vector2[] positions;

    [FormerlySerializedAs("heroSquadPrefab")] public HeroSquadMono heroSquadMonoPrefab;
    

    public void NewHeroSquadOnAdventure(HeroSquad squad)
    {

        if (heroSquads.Count >= positions.Length)
        {
            return;
        }

        var allIndex = heroSquads.Keys.ToArray();

        var ii = 0;

        for (var i = 0; i < allIndex.Length; i++)
        {
            var intv = allIndex[i];
            if (ii < intv)
            {
                ii = i;
                break;
            }
            
            ii = i + 1;
        }
        
        Debug.Log($"#Adventure# index{ii}");
        
        var position = positions[ii];
            
        var heroSquad = Instantiate(heroSquadMonoPrefab, position, Quaternion.identity, transform);

        heroSquad.NewSquad(squad);
        
        heroSquad.indexInAdventure = ii;
        
        heroSquad.transform.localPosition = position;
        
        // 怪物被击杀
        heroSquad.enemyDefeated += EnemyDidDefeated;
        heroSquad.dungeonDidFinished += DungeonDidFinished;

        heroSquads[ii] = heroSquad;
        
        var dungeon = dungeonSO.toDungeon;
            
        // 开始`dungeon`
        heroSquad.DidEnterDungeon(dungeon);

    }
    
    
    
    [Button, DisableInEditorMode]
    public void NewDungeon()
    {

        // if (heroSquads.IsNullOrEmpty())
        // {
        //     GenerateHeroSquad();
        // }
        //
        //
        // foreach (var heroSquad in heroSquads)
        // {
        //     var dungeon = dungeonSO.toDungeon;
        //
        //     // 怪物被击杀
        //     heroSquad.enemyDefeated += EnemyDidDefeated;
        //     
        //     // 开始`dungeon`
        //     heroSquad.DidEnterDungeon(dungeon);
        // }
    }

    private void GenerateHeroSquad()
    {
        // heroSquads = new HeroSquad[positions.Length];
        //
        // for (var i = 0; i < positions.Length; i++)
        // {
        //     var position = positions[i];
        //     
        //     var heroSquad = Instantiate(heroSquadPrefab, position, Quaternion.identity, transform);
        //
        //     heroSquad.transform.localPosition = position;
        //
        //     // 怪物被击杀
        //     heroSquad.enemyDefeated += EnemyDidDefeated;
        //
        //     heroSquads[i] = heroSquad;
        // }
        
    }

    private void EnemyDidDefeated(EnemyMono eMono)
    {
        Destroy(eMono.gameObject);
    }

    // 冒险完成
    private void DungeonDidFinished(HeroSquadMono heroSquadMono, Dungeon dungeon)
    {
        
        // heroSquads.Remove(heroSquadMono.indexInAdventure);
        //
        // Destroy(heroSquadMono.gameObject);
        
    }
}
