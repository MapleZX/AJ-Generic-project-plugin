using System;
using UnityEngine;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    [Serializable]
    public class AJItemModel : AJModel
    {
        [SerializeField, CustomLabel("Is Stack")] private bool _isStack;
        [SerializeField, CustomLabel("Level")] private int _level;
        private int _stack = 0;
        public int StackMax { get; set; }
        public int LevelMax { get; set; }
        public bool IsStack => _isStack;
        public int Level {
            get => _level;
            set {
                _level = value;
                if (value > LevelMax)
                    _level = LevelMax;
            }
        }
        public int Stack { 
            get => _stack; 
            set {
                if (!_isStack)
                {
                    _stack = 1;
                } else
                {
                    _stack = value;
                    if (value > StackMax)
                        _stack = StackMax;
                }
            }
        }
    }
}
