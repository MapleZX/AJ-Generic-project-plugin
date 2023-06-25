using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Tools.Keys;

namespace AJ.Generic.Tools
{
    public sealed class LoadProgressBar : MonoBehaviour
    {
        [SerializeField] private SceneKey sceneKey = SceneKey.MainScene;
        public event Func<bool> Confirm;
        private UIController controller;
        private ProgressBar progressBar;
        private AJUIInfo ajElement;
        [HideInInspector] public bool isLoadScene = false;
        void Start() 
        {           
            controller = GetComponentInParent<UIController>();
            controller.transform.parent = SlimeManagerController.Instance.transform;
            ajElement = AJUIInfo.CreateAJElement(controller);
            StartCoroutine(ILoadScene());
            AJController.RegisterAJMessage<AJMessage>("LoadSceneCompleted", OnLoadSceneCompleted);
        }
        void OnDestroy()
        {
            AJController.UnRegisterAJMessage<AJMessage>("LoadSceneCompleted", OnLoadSceneCompleted);
        }
        private void OnLoadSceneCompleted(AJMessage message)
        {
            Destroy(controller.gameObject);
        }
        private IEnumerator ILoadScene()
        {
            yield return new WaitUntil(() => controller.Status == ControllerStatus.Succeeded);         
            var screen = controller.root.Q<VisualElement>("main-screen");
            progressBar = controller.root.Q<ProgressBar>("load-progress");
            screen.style.display = DisplayStyle.Flex;           
            StartCoroutine(IProgressBar());
            yield return new WaitUntil(() => isLoadScene);
            LevelManager.Instance.LoadSceneAsync(sceneKey, LoadCompleted);
        }
        private bool LoadCompleted()
        {
            if (Confirm == null) return true;
            return Confirm.Invoke();
        }
        private IEnumerator IProgressBar()
        {
            yield return new WaitUntil(() => LevelManager.Instance.IsRun);
            while(LevelManager.Instance.progress < 90)
            {
                yield return new WaitForSecondsRealtime(Time.smoothDeltaTime);
                var value = LevelManager.Instance.progress * 100;
                progressBar.title = value >= 90 ? "100%" : value + "%";
                progressBar.value = value >= 90 ? 100 : value;
            }
        }
    }
}
