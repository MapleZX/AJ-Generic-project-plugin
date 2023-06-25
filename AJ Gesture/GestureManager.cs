using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools.Gesture
{
    public class GestureManager : MonoBehaviour
    {
        
        [SerializeField] private RegisterNameIngredient registerNameIngredient = new();
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        private DollarRecognizer dollar = new();
        public DollarRecognizer Dollar => dollar;
        void Awake() => AJController.Register<GestureManager>(RegisterName, gameObject);
        void OnDestroy() => AJController.UnRegister<GestureManager>(RegisterName);
        private void CreateDollarObject()
        {
            if (dollar == null) dollar = new();
        }
        public void SavePattern(TextAsset xmlText)
        {
            var text = xmlText.text;
            var doc = new XmlDocument();
            doc.LoadXml(text);
            var node = doc.SelectSingleNode("Gesture");
            var nodes = node.ChildNodes;
            var points = new List<Vector2>();
            for (int i = 0; i < nodes.Count; i++)
            {
                var x = float.Parse(nodes[i].Attributes["X"].Value);
                var y = float.Parse(nodes[i].Attributes["Y"].Value);
                points.Add(new Vector2(x, y));
            }
            CreateDollarObject();
            dollar.SavePattern(node.Attributes["Name"].Value, points); 
        }
        public void SavePattern(string xml, string xmlName)
        {
            var text = xml;
            var doc = new XmlDocument();
            doc.LoadXml(text);
            var node = doc.SelectSingleNode("Gesture");
            var nodes = node.ChildNodes;
            var points = new List<Vector2>();
            for (int i = 0; i < nodes.Count; i++)
            {
                var x = float.Parse(nodes[i].Attributes["X"].Value);
                var y = float.Parse(nodes[i].Attributes["Y"].Value);
                points.Add(new Vector2(x, y));
            }
            CreateDollarObject();
            dollar.SavePattern(xmlName, points);
        }
        public DollarRecognizer.Result Recognize(IEnumerable<Vector2> points)
        {
            var result = dollar.Recognize(points);
            return result;
        }
        public void Recognize(IEnumerable<Vector2> points, Action<DollarRecognizer.Result> gestureCallbacks)
        {
            var result = dollar.Recognize(points);
            gestureCallbacks.Invoke(result);
        }
        public void Recognize(IEnumerable<Vector2> points, Action<DollarRecognizer.Unistroke> gestureCallbacks)
        {
            var result = dollar.Recognize(points);
            if (result.Match != null)
                gestureCallbacks.Invoke(result.Match);
        }
        public void Recognize(IEnumerable<Vector2> points, Action<string> gestureCallbacks)
        {
            var result = dollar.Recognize(points);
            if (result.Match != null)
                gestureCallbacks.Invoke(result.Match.Name);
        }
    }
}