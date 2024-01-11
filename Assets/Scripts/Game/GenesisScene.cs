using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenesisScene : MonoBehaviour
{

    public Button loadGameButton;
    
    public void NewGame()
    {
        ES3.DeleteFile("SaveFile.es3");
        SceneManager.LoadScene("RandomBeginning");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }


    private void Start()
    {
        loadGameButton.interactable = ES3.FileExists("SaveFile.es3");
    }
}
