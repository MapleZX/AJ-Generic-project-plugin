using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AJ.Generic.Configuration
{
    [CreateAssetMenu(fileName = "DemoSceneConfigure", menuName = "AJ Generic Tools/DemoSceneConfigure", order = 2)]
    public class DemoScene : ScriptableObject
    {
        public SceneAsset initializeScene;
        public List<SceneAsset> demos;
    }
}
