#if UNITY_EDITOR
    
using Tyrant;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public static class GameInEditor
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // var scene = SceneManager.GetActiveScene();
            // if (!scene.name.Equals(LoadingScenes.Scene.Genesis.ToString()))
            // {
            //     SceneManager.LoadScene(LoadingScenes.Scene.Genesis.ToString());
            // }
        }
    
    }
}
#endif