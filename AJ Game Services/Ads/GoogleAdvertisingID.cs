using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdvertisingID : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var advertisingID = GetGoogleAdvertisingID();
        Debug.Log($"广告ID: {advertisingID}");
    }
    public string GetGoogleAdvertisingID()
    {
        var advertisingID = "default";
        var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        var jc2 = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
        var jo2 = jc2.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", jo);
        if (jo2 != null)
        {
            advertisingID = jo2.Call<string>("getId");
            var adTrackLimited = jo2.Call<bool>("isLimitAdTrackingEnabled");
            var code = jo2.Call<int>("isGooglePlayServicesAvailable", jo);
        }
        return advertisingID;
    }
}
