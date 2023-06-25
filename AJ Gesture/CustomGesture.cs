using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools.Gesture
{
    [System.Serializable]
    public class CustomGesture
    {
        public string Name;
        public List<Vector2> points;

        public void SavePattern(DollarRecognizer dollar)
        {
            dollar.SavePattern(Name, points);
        }
    }
}
