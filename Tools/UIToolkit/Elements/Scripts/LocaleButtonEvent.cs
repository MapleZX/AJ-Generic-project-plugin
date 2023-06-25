using UnityEngine;
using UnityEngine.SceneManagement;
using AJ.Generic.Tools.Keys;

namespace AJ.Generic.Tools
{
    [RequireComponent(typeof(LocaleSelectButton))]
    public class LocaleButtonEvent : MonoBehaviour
    {
        [SerializeField] private bool resetScene = true;
        void Start() 
        {
            var localeBtn = GetComponent<LocaleSelectButton>();
            if (resetScene) localeBtn.clicked += ChangeScene;
        }
        [SerializeField] private SceneKey sceneKey;
        public void ChangeScene()
        {
            SceneManager.LoadScene((int)sceneKey);
        }
        public void ChangeScene(CountryCode countryCode)
        {
            SceneManager.LoadScene((int)sceneKey);
        }
    }
}
