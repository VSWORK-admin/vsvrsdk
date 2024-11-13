using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FfmpegUnity.Sample
{
    public class Menu : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void ChangeScene(string sceneName)
        {
            BackButton.PrevSceneName = gameObject.scene.path;
            SceneManager.LoadScene(sceneName);
        }
    }
}
