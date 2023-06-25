using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace AJ.Generic.Configuration
{
    public class InitializeOnLoad : MonoBehaviour
    {
        // public static string sceneName { get; private set; }
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            var config = AJConfiguration.Configuration();
            var demoScene = config.DemoScene;
            var demos = demoScene.demos;
            if (demos.Any())
            {
                foreach (var demo in demos)
                {
                    if (demo != null)
                    {
                        if (demo.name == SceneManager.GetActiveScene().name)
                        {
                            Debug.Log("This is Demo Scene!");
                            return;
                        }
                    }
                }
            }
            // sceneName = SceneManager.GetActiveScene().name;
            if (demoScene.initializeScene == null)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    return;
                }
                SceneManager.LoadScene(0);
            } else
            {
                if (SceneManager.GetActiveScene().name == demoScene.initializeScene.name)
                {
                    return;
                }
                SceneManager.LoadScene(demoScene.initializeScene.name);
            }                                   
        }
    }
}
