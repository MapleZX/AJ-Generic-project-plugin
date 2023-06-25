using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools.Gesture
{  
    public class MagicPaper : MonoBehaviour
    {
        [SerializeField] private RegisterNameIngredient registerNameIngredient = new();
        [SerializeField] private bool canDraw = true;
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        private Dictionary<string, Transform> positionings = new();
        private LineRenderer[] lineRenderers;   
        private bool isReleaseMagic = false;
        private int _paintbrush = -1;
        public int Paintbrush {
            get {
                if (_paintbrush < lineRenderers.Length) return _paintbrush;
                else return _paintbrush - 1;
            }
        }
        public bool CanDraw { get => canDraw; set => canDraw = value; }
        public bool IsReleaseMagic { get => isReleaseMagic; set => isReleaseMagic = value; }
        void Awake() => AJController.Register<MagicPaper>(RegisterName, gameObject);
        void Start() => Initialization();
        void OnDestroy() => AJController.UnRegister<MagicPaper>(RegisterName);   
        private void Initialization()
        {
            GetPositionings();
            GetLineRenderer();
        }
        void Update()
        {
            if (!canDraw) return;
            if (_paintbrush >= lineRenderers.Length)
            {
                RefreshPaper();
                return;
            }
            if (Input.GetMouseButtonDown(0) && InPaper(out var newPos))
            {
                if (_paintbrush < 0) _paintbrush = 0;
                lineRenderers[_paintbrush].SetPosition(0, newPos);
                lineRenderers[_paintbrush].SetPosition(1, newPos);
            } else if (Input.GetMouseButton(0) && InPaper(out var newMovePos))
            {
                if (_paintbrush < 0) _paintbrush = 0;
                lineRenderers[_paintbrush].positionCount++;
                var index = lineRenderers[_paintbrush].positionCount;
                lineRenderers[_paintbrush].SetPosition(index - 1, newMovePos);
            } else if (Input.GetMouseButtonUp(0) && InPaper(out var nonePos))
            {        
                _paintbrush++;
                if (_paintbrush >= lineRenderers.Length) 
                {
                    canDraw = false;
                }
            }
        }
        private void GetPositionings()
        {
            var positioning = transform.Find("positioning");
            var childNodeCount = positioning.childCount;
            for (int i = 0; i < childNodeCount; i++)
            {
                var child = positioning.GetChild(i);
                positionings[child.name] = child;
            }
        }
        private void GetLineRenderer()
        {
            var line = transform.Find("Line");
            var childCount = line.childCount;
            lineRenderers = new LineRenderer[childCount];
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i] = line.GetChild(i).GetComponent<LineRenderer>();
                var newPos = lineRenderers[i].transform.position;
                lineRenderers[i].transform.position = new Vector3(newPos.x, newPos.y, -9);
            }
        }
        public Transform GetPositioning(string key)
        {
            var have = positionings.TryGetValue(key, out var child);
            return child;
        }
        public void RefreshPaper()
        {
            foreach (var line in lineRenderers)
            {
                line.positionCount = 0;
                line.positionCount = 2;
            }
            _paintbrush = -1;
            canDraw = true;
        }
        public Vector2[] GetLinePositions(int paintbrush)
        {
            if (paintbrush >= _paintbrush) return null;
            if (paintbrush < 0) return null;
            var newPoints = new Vector3[lineRenderers[paintbrush].positionCount];
            lineRenderers[paintbrush].GetPositions(newPoints);
            var newPoints2 = new Vector2[newPoints.Length];
            for (int i = 0; i < newPoints2.Length; i++)
            {
                newPoints2[i] = new Vector2(newPoints[i].x, newPoints[i].y);
            }
            return newPoints2;
        }
        public List<Vector2[]> GetLinesPositions()
        {
            if (Paintbrush <= 0) return null;
            var lines = new List<Vector2[]>();
            for (int i = 0; i < Paintbrush; i++)
            {
                var newPos = GetLinePositions(i);
                lines.Add(newPos);
            }
            return lines;
        }
        public bool InPaper()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y);
            return InPaper(pos);
        }
        public bool InPaper(out Vector3 newPoint)
        {
            newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPoint = new Vector3(newPoint.x, newPoint.y);
            return InPaper(newPoint);
        }
        public bool InPaper(Vector3 position)
        {
            var maxX = positionings["Right"].position.x;
            var minX = positionings["Left"].position.x;
            var maxY = positionings["Top"].position.y;
            var minY = positionings["Bottom"].position.y;
            var X = position.x;
            var Y = position.y;
            if (X < maxX && X > minX && Y < maxY && Y > minY)
            {
                return true;
            }
            return false;
        }
        public float RegionalProportion(Vector2[] positions, RegionStatus regionStatus)
        {
            var total = 1;
            var regions = Regions(positions, out total);
            var proportion = (regions[regionStatus] / (float)total);
            return proportion;
        }
        public Dictionary<RegionStatus, int> Regions(Vector2[] positions, out int total)
        {
            var count = new Dictionary<RegionStatus, int>();
            var enumList = Enum.GetValues(typeof(RegionStatus)).Cast<RegionStatus>().ToList();
            total = 0;
            foreach (var e in enumList)
            {
                count[e] = 0;
            }
            foreach (var p in positions)
            {
                var region = Region(p);
                count[region]++;
                total++;
            }
            return count;
        }
        public RegionStatus Region(Vector3 position)
        {
            var maxX = positionings["Right"].position.x;
            var minX = positionings["Left"].position.x;
            var maxY = positionings["Top"].position.y;
            var minY = positionings["Bottom"].position.y;
            var centerX = positionings["Center"].position.x;
            var centerY = positionings["Center"].position.y;
            var X = position.x;
            var Y = position.y;
            var status = RegionStatus.Center;
            if (X > centerX && X < maxX && Y > centerY && Y < maxY)
            {
                status = RegionStatus.UpperRight;
            } else if (X < centerX && X > minX && Y > centerY && Y < maxY)
            {
                status = RegionStatus.UpperLeft;
            } else if (X < centerX && X > minX && Y < centerY && Y > minY)
            {
                status = RegionStatus.LowerLeft;
            } else if (X > centerX && X < maxX && Y < centerY && Y > minY)
            {
                status = RegionStatus.LowerRight;
            } else if (X == centerX && Y > centerY && Y < maxY)
            {
                status = RegionStatus.Top;
            } else if (X == centerX && Y < centerY && Y > minY)
            {
                status = RegionStatus.Bottom;
            } else if (X > centerX && X < maxX && Y == centerY)
            {
                status = RegionStatus.Right; 
            } else if (X < centerX && X > minX && Y == centerY)
            {
                status = RegionStatus.Left;
            } else if (X == centerX && Y == centerY)
            {
                status = RegionStatus.Center;
            }
            return status;
        }
    }
    public enum RegionStatus 
    { 
        None, 
        UpperLeft, 
        Top,
        UpperRight, 
        Left, 
        Center, 
        Right, 
        LowerLeft, 
        Bottom,
        LowerRight 
    }
}