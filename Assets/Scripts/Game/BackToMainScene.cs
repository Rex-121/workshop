using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tyrant
{
    public class BackToMainScene: MonoBehaviour
    {

        public void BackToMain()
        {

            SceneManager.LoadScene("Genesis");

        }
        
        
    }
}