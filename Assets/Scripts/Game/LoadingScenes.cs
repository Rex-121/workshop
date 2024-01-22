using System;
using UniRx;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tyrant
{
    public class LoadingScenes: MonoBehaviour
    {
        
        public enum Scene
        {
            Genesis, SampleScene, RandomBeginning
        }

        public BlackSmith.BlackSmith blackSmith;
        
        public float animationDuration = 500;

        public Scene scene;
        private void Start()
        {
            if (UIManager.main != null)
            {
                UIManager.main.gameObject.SetActive(false);
            }
            
            StartCoroutine(Load());
        }

        private void End(AsyncOperation operation)
        {
            blackSmith.HammerNow();
            Observable.Timer(TimeSpan.FromMilliseconds(animationDuration))
                .Take(1)
                .TakeUntilDestroy(this)
                .Subscribe(v =>
                {
                    operation.allowSceneActivation = true;
                }).AddTo(this);
        }

        // TODO: addressable
        private IEnumerator Load()
        {
            var operation = SceneManager.LoadSceneAsync(scene.ToString());
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    End(operation);
                    yield break;
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}