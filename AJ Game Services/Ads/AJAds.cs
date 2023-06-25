using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
namespace AJ.Generic.Service
{
    public class AJAds
    {
        private AJAds(Action<InitializationStatus> mediationAction = null)
        {
            if (TurnOff)
            {
                Debug.Log("广告已经关闭!");
                return;
            }
            RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
                .SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.True)
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus => {
                mediationAction?.Invoke(initStatus);
            });
        }
        private static AJAds _instance;
        public static AJAds Instance => _instance;
        private Dictionary<string, IAJAd> ads = new();
        private static bool isTurnOff = false;
        public static bool TurnOff { 
            get => isTurnOff; 
            set {
                isTurnOff = value;
                if (!value) return;
                if (Instance == null) return;
                foreach(var ad in Instance.ads.Values)
                {
                    ad.RemoveAd();
                }
                Instance.ads.Clear();
            }  
        }
        public static AJAds Activate(bool mediation)
        {          
            if (_instance != null) return _instance;
            if (mediation) _instance = new(mediationAction);
            else _instance = new();
            return _instance;
        }
        private static void mediationAction(InitializationStatus initStatus)
        {
            Debug.Log(System.String.Format("启动第三方中介模式！"));
            var map = initStatus.getAdapterStatusMap();
            foreach (var keyValuePair in map)
            {
                var className = keyValuePair.Key;
                var status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        }
        public void AddAd(string adUnitId, IAJAd ad)
        {
            ads.Add(adUnitId, ad);
        }
        public T GetAd<T>(string adUnitId) where T : class, IAJAd
        {
            if (ads.ContainsKey(adUnitId))
            {
                return ads[adUnitId] as T;
            }
            return default;
        }
    }
}
