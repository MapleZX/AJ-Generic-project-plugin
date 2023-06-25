using System;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Utils;
using AJ.Generic.Extension;

namespace AJ.Generic.Tools.Gesture
{
    [DisallowMultipleComponent]
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] private RegisterNameIngredient registerNameIngredient = new();
        [SerializeField] private TouchStatus touchStatus;
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        public event Action<Collider2D> TargetCollider2D;
        public event Action<RaycastHit2D> TargetRaycastHit2D;
        private bool hasCollider2D;
        public bool HasCollider2D => hasCollider2D;
        private Dictionary<TouchStatus, Action> targetActions = new();
        void Awake() => AJController.Register<TargetManager>(RegisterName, gameObject);
        void OnDestroy() => AJController.UnRegister<TargetManager>(RegisterName);
        void Start()
        {
            targetActions[TouchStatus.Touch] = TouchRaycast;
            targetActions[TouchStatus.Up] = TouchRaycastUp;
            targetActions[TouchStatus.Down] = TouchRaycastDown;
        }
        // Update is called once per frame
        void Update()
        {
            if (this.IsGamesPause(typeof(TargetManager))) return;
            if (this.IsPause()) return;
            targetActions[touchStatus].Invoke();
        }
        private void TouchRaycast()
        {
            if (Input.GetMouseButton(0))
            {
                TouchRaycastAction();
            }
        }
        private void TouchRaycastUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                TouchRaycastAction();
            }
        }
        private void TouchRaycastDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchRaycastAction();
            }
        }
        private void TouchRaycastAction()
        {
            var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(origin, Vector2.zero);
            TargetRaycastHit2D?.Invoke(hit);
            hasCollider2D = false;
            Debug.LogFormat("发射射线 {0}", hit.normal);
            if (hit.collider != null)
            {
                TargetCollider2D?.Invoke(hit.collider);
                hasCollider2D = true;
            }      
        }
    }
    public enum TouchStatus
    {
        Touch = -1,
        Up = 0,
        Down = 1
    }
}