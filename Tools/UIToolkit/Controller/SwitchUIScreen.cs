using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools
{
    [DisallowMultipleComponent]
    public sealed class SwitchUIScreen : MonoBehaviour, IScreenSwitch, IManager<SwitchUIScreen>
    {
        [SerializeField] private bool isCustomRegisterName = false;
        [SerializeField] private string registerName = "";
        [SerializeField] private bool isMainScreenSwitch = false;
        public string RegisterName => isMainScreenSwitch ? MainScreenName : (!isCustomRegisterName ? name : registerName);
        public const string MainScreenName = "AJ.Generic.Tools-MainScreenSwitch-UIController-2023-SSS";
        private string selectedScreen;
        public string SelectedScreen => selectedScreen;
        public bool IsMainScreenSwitch => isMainScreenSwitch;
        private event Action<string, bool> _switchEvent;
        event Action<string, bool> IScreenSwitch.SwitchEvent { add => _switchEvent += value; remove => _switchEvent -= value; }
        void Awake() 
        {
            AJController.Register<SwitchUIScreen>(RegisterName, gameObject);
        }
        void OnDestroy() => AJController.UnRegister<SwitchUIScreen>(RegisterName);
        SwitchUIScreen IManager<SwitchUIScreen>.Manager => this;
        public void SwitchPage(string page, bool cover)
        {
            selectedScreen = page;
            _switchEvent?.Invoke(selectedScreen, cover);
        }
    }
}
