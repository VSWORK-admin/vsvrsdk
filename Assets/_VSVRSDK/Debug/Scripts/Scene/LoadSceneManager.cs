using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace VSWorkSDK
{
    public class LoadSceneManager : MonoBehaviour
    {
        public int SceneIndex = 0;
        public bool bLoadOnStart = false;

        public InputField InputField_SceneIndex;
        // Start is called before the first frame update
        void Start()
        {
            if(bLoadOnStart)
            {
                StartCoroutine(LoadScene(SceneIndex, 2));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ClickLoadSceneByIndex()
        {
            LoadSceneByIndex(InputField_SceneIndex.text.ToInt(),1);
        }

        public void LoadSceneByIndex(int index,int delayTime)
        {
            StartCoroutine(LoadScene(index, delayTime));
        }

        public IEnumerator LoadScene(int index,int time)
        {
            yield return new WaitForSeconds(time);

            if (index < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(index, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError("LoadScene Error SceneIndex >= SceneManager.sceneCountInBuildSettings SceneIndex : " + SceneIndex);
            }
        }

        internal void LoadNextScene(WsSceneInfo nowRoomLinkScene, bool v)
        {
            if (nowRoomLinkScene == null) return;

            SceneManager.LoadScene(nowRoomLinkScene.name, LoadSceneMode.Single);
        }

        internal void LoadDefaultStartScene()
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }

        internal void CleanCachesAndGC()
        {
            Caching.ClearCache();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        internal void CleanCaches()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}