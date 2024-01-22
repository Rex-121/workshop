using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tyrant
{
    public class BackToMainScene: MonoBehaviour
    {

        public LoadingScenes loadingScenesPrefab;
        
        public void BackToMain()
        {

            var loadingScenes = Instantiate(loadingScenesPrefab);
            loadingScenes.scene = LoadingScenes.Scene.Genesis;

        }


        
        
    }
}