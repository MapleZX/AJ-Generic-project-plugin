using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools.Gesture
{
    [System.Serializable]
    public class CustomGestures
    {
        public CustomGestures()
        {
            customGestures = new();
        }
        public List<CustomGesture> customGestures;
        public void SavePattern(DollarRecognizer dollar)
        {
            if (!customGestures.Any()) return;
            foreach (var gesture in customGestures)
            {
                dollar.SavePattern(gesture.Name, gesture.points);
            }
        }
    }
}
