using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Tools;

namespace AJ.Generic.Tools.Gesture
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LineRenderer))]
    public class GestureTemplateDraw : MonoBehaviour
    {
        private string TemplateXmlTitle = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
        private string TemplateXmlSubject = 
        @"<Gesture Name={0}>
{1}
</Gesture>";
        private string TemplateXmlPoint = "<Point X=\"{0}\" Y=\"{1}\"/>";
        private LineRenderer lineRenderer;
        private List<Vector2> points = new();
        private Dictionary<string, AJTextField> textFields = new();
        private AJPanel panel;
        private Dictionary<string, AJButton> buttons = new();
        private bool isDraw = false;
        private int addCount = 0;
        [SerializeField] private string TemplatesPath = "/AJ Gesture/Gesture Templates/";
        [SerializeField] private int addMaxCount = 2;
        // Start is called before the first frame update
        void Start()
        {
            var path = Application.dataPath + TemplatesPath;
            lineRenderer = GetComponent<LineRenderer>();
            panel = transform.Find("CreatePanel").GetComponent<AJPanel>();
            textFields["Input"] = transform.Find("Input").GetComponent<AJTextField>();
            textFields["Input1"] = transform.Find("Input1").GetComponent<AJTextField>();
            buttons["Create"] = transform.Find("Create").GetComponent<AJButton>();
            buttons["Save"] = transform.Find("Save").GetComponent<AJButton>();
            buttons["Clear"] = transform.Find("Clear").GetComponent<AJButton>();

            panel.OnTouch += () => {
                isDraw = false;
            };

            panel.OutsideTouch += () => {
                isDraw = true;
            };

            buttons["Create"].OnClick += () => {
                isDraw = true;
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = 2;            
            };

            buttons["Save"].OnClick += () => {
                if (textFields["Input"].Text == "") return;          
                var xmlText = new System.Text.StringBuilder();
                xmlText.AppendLine(TemplateXmlTitle);
                var pointText = new System.Text.StringBuilder();
                foreach (var point in points)
                {
                    addCount--;
                    if (addCount <= 0)
                    {
                        var text = "\t" + System.String.Format(TemplateXmlPoint, point.x, point.y);
                        pointText.AppendLine(text);
                        addCount = addMaxCount;
                    }
                }
                var subject = System.String.Format(TemplateXmlSubject, "\"" + textFields["Input"].Text + "\"", pointText);
                xmlText.Append(subject);
                var scriptPath = path + textFields["Input"].Text + textFields["Input1"].Text + ".xml";
                using (FileStream file = new FileStream(scriptPath, FileMode.Create))
                {
                    using (StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8))
                    {
                        fileW.Write(xmlText);
                        fileW.Flush();
                    }
                }
                // GestureManager.Instance.SavePattern(xmlText.ToString(), textFields["Input"].Text);
                points.Clear();
            };

            buttons["Clear"].OnClick += () => {
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = 2;
                textFields["Input"].Text = "";
                textFields["Input1"].Text = "";
                points.Clear();
            };
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!isDraw) return;
            if (Input.GetMouseButtonDown(0))
            {
                var pos = AddPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, pos);
                lineRenderer.SetPosition(1, pos);
            } else if (Input.GetMouseButton(0))
            {
                var pos = AddPoint(Input.mousePosition);
                lineRenderer.positionCount++;
                var index = lineRenderer.positionCount - 1;
                lineRenderer.SetPosition(index, pos);
            } else if (Input.GetMouseButtonUp(0))
            {
                isDraw = false;
                // var result = GestureManager.Instance.Recognize(points);
                // if (result.Match != null)
                // {
                //     textFields["Input"].Text = result.Match.Name;
                //     Debug.Log(result.ToString());
                // }
            }
        }
        private Vector2 AddPoint(Vector3 pos)
        {
            var p = Camera.main.ScreenToWorldPoint(pos);
            var point = new Vector2(p.x, p.y);
            points.Add(new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)));     
            return point;
        }
    }
}
