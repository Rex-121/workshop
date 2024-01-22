using System;
using System.Collections;
using System.Collections.Generic;
using Tyrant;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenesisScene : MonoBehaviour
{

    public Button loadGameButton;

    public LoadingScenes loadingScenesPrefab;
    public void NewGame()
    {
        
        
        Storage.main.DeleteData();
        var loadingScenes = Instantiate(loadingScenesPrefab);
        loadingScenes.scene = LoadingScenes.Scene.RandomBeginning;
        
        
        gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        var loadingScenes = Instantiate(loadingScenesPrefab);
        loadingScenes.scene = LoadingScenes.Scene.SampleScene;
        
        
        gameObject.SetActive(false);
    }


    private void Start()
    {
        loadGameButton.interactable = ES3.FileExists("SaveFile.es3");
    }
}
