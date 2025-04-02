using com.unimob.pattern.singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS.Pattern.Observer;
using System;

public class DeviceManager : MonoSingleton<DeviceManager>
{
    public bool InBlackList = false;
    public string Id = string.Empty;

    private BlackListDevice blackList;

    public void Init(Transform parent = null)
    {
        AppManager.Device = this;
        if (parent) transform.SetParent(parent);

        var blackList = Resources.Load<BlackListDevice>("BlackListDevice");
        if (blackList != null)
            this.blackList = blackList;

        Id = GetDeviceId().ToLower();
        //Debug.Log($"Device Id: {Id}");

        InBlackList = blackList.InBlacklist(Id);

        this.RegisterListener(EventID.RemoteConfigComplete, RemoteConfigComplete);
    }

    private void RemoteConfigComplete(object obj)
    {
        this.RemoveListener(EventID.RemoteConfigComplete, RemoteConfigComplete);
        blackList.UpdateData(DataManager.Save.RemoteConfig.BlacklistConfigValue);
        InBlackList = blackList.InBlacklist(Id);
    }

    public static string GetDeviceId()
    {
        if (CheckForSupportedMobilePlatform())
        {
#if UNITY_ANDROID
            AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
            return clsSecure.CallStatic<string>("getString", objResolver, "android_id");
#endif

#if UNITY_IPHONE
            return UnityEngine.iOS.Device.vendorIdentifier;
#endif
        }
        else
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    private static bool CheckForSupportedMobilePlatform()
    {
#if UNITY_EDITOR
        return false;
#endif

        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}