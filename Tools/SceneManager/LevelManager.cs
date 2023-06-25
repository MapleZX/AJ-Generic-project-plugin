using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using AJ.Generic.Tools.Singleton;
using AJ.Generic.Tools.Keys;
using AJ.Generic.Extension;
namespace AJ.Generic.Tools
{
    /// <summary>
    /// 场景切换单例。
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        public float progress { get; private set; }
        public bool IsRun => _run;
        private bool _active = false;
        private bool _run = false;
        void Awake() => _Awake();
        public void LoadSceneAsync(SceneKey sceneKey, Func<bool> succeed, bool useAsync = false)
        {
            if (_active) return;
            _active = true;
            _run = true;
            progress = 0;
            if (useAsync) LoadSceneAsync(sceneKey, succeed);
            else StartCoroutine(ILoadScene(sceneKey, succeed));
        }
        private void LoadCompleted(AsyncOperation asyncOperation)
        {
            asyncOperation.allowSceneActivation = true;
            _active = false;
            _run = false;
        }
        /// <summary>
        /// 协程加载新场景
        /// </summary>
        /// <param name="sceneKey">场景名称</param>
        /// <returns></returns>
        private IEnumerator ILoadScene(SceneKey sceneKey, Func<bool> succeed)
        {        
            yield return null;            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneKey.ToString());
            asyncOperation.allowSceneActivation = false;
            progress = 0;
            _active = true;
            _run = true;

            while (!asyncOperation.isDone)
            {
                progress = asyncOperation.progress;
                Debug.LogFormat("Loading {0} : {1}%", sceneKey, Mathf.Round(asyncOperation.progress * 100));
                if (asyncOperation.progress >= 0.9f)
                {
                    Debug.Log("Loading completed!");
                    if (succeed())
                    {
                        LoadCompleted(asyncOperation);
                    }
                }
                yield return null;
            }
        }
        /// <summary>
        /// 异步加载新场景
        /// </summary>
        /// <param name="sceneKey">场景名称</param>
        private async void LoadSceneAsync(SceneKey sceneKey, Func<bool> succeed)
        {           
            var asyncOperation = SceneManager.LoadSceneAsync(sceneKey.ToString());
            asyncOperation.allowSceneActivation = false;
            progress = 0;
            _active = true;
            _run = true;
            do
            {
                progress = asyncOperation.progress;
                Debug.Log(sceneKey + ":" + Mathf.Round(asyncOperation.progress * 100) + "%");
            } while (asyncOperation.progress < 0.9f);
            await Task.Delay(0);
            Debug.Log("Loading completed!");          
            if (succeed())
            {
                LoadCompleted(asyncOperation);
            }
        }

        #region 弃用
        [Obsolete]
        /// <summary>
        /// 异步卸载当前场景
        /// </summary>
        /// <param name="sceneKey">场景名称</param>
        /// <param name="options">卸载场景选项</param>
        public async void UnLoadSceneAsync(SceneKey sceneKey, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
        {
            var scene = SceneManager.GetSceneByName(sceneKey.ToString());
            if (scene.isLoaded)
            {
                var asyncOperation = SceneManager.UnloadSceneAsync((int)sceneKey, options);
                Debug.Log("Unload This Scene:" + sceneKey.ToString());
            }
            else
            {
                Debug.Log("Not loaded This Scene:" + sceneKey.ToString());
            }
            await Task.Delay(0);
        }
        [Obsolete]
        public async void LoadScene(int sceneBuildIndex)
        {
            var target = 0f;
            // progressBar.fillAmount = 0;

            var scene = SceneManager.LoadSceneAsync(sceneBuildIndex);
            scene.allowSceneActivation = false;

            // loadCanvas.SetActive(true);

            do
            {
                // await Task.Delay(100);
                target = scene.progress;

            } while (scene.progress < 0.9f);

            await Task.Delay(0);

            scene.allowSceneActivation = true;

            // loadCanvas.SetActive(false);
        }
        [Obsolete]
        public async void LoadScene(string sceneName)
        {
            var target = 0f;
            // progressBar.fillAmount = 0;

            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;

            // loadCanvas.SetActive(true);
            do
            {
                // await Task.Delay(100);
                target = scene.progress;

            } while (scene.progress < 0.9f);

            await Task.Delay(0);

            scene.allowSceneActivation = true;

            // loadCanvas.SetActive(false);
        }
        #endregion
    }
}
