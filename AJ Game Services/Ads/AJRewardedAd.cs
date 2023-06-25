using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
namespace AJ.Generic.Service
{
    public class AJRewardedAd : IAJAd
    {
        private string _adUnitId;
        private LoadAdError _adError;
        private RewardedAd _ad;
        public RewardedAd Ad => _ad;
        public string AdUnitId => _adUnitId;
        public LoadAdError AdError => _adError;
        private AJRewardedAd(string adUnitId)
        {
            this._adUnitId = adUnitId;
        }
        public static AJRewardedAd Activate(string adUnitId)
        {
            if (AJAds.Instance == null) return null;
            var ad = AJAds.Instance.GetAd<AJRewardedAd>(adUnitId);
            if (ad != null) return ad;
            ad = new AJRewardedAd(adUnitId);
            AJAds.Instance.AddAd(adUnitId, ad);
            return ad;
        }
        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            RemoveAd();
            if (AJAds.TurnOff) return;
            Debug.Log("Loading the rewarded ad.");
            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();
            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest, (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " + "with error : " + error);
                    _adError = error;
                    return;
                }
                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _ad = ad;
            });
        }
        public void AdFullScreenContentClosed(Action closed)
        {
            // Raised when the ad closed full screen content.
            Ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");
                closed?.Invoke();
                // Reload the ad so that we can show another as soon as possible.
                // LoadRewardedAd();
            };
        }
        public void AdFullScreenContentFailed(Action<AdError> failed)
        {
            // Raised when the ad failed to open full screen content.
            Ad.OnAdFullScreenContentFailed += error =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
                failed?.Invoke(error);
                // Reload the ad so that we can show another as soon as possible.
                // LoadRewardedAd();
            };
        }
        public void ShowRewardedAd(Action<string, double> rewardAction)
        {
            const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
            if (Ad != null && Ad.CanShowAd())
            {
                Ad.Show(reward =>
                {
                    // TODO: Reward the user.
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                    rewardAction?.Invoke(reward.Type, reward.Amount);
                });
            }
        }
        public void RemoveAd()
        {
            if (_ad != null)
            {
                _ad.Destroy();
                _ad = null;
            }
        }
    }
}
