using com.unimob.services.remote_config;
using TMPro;
using UnityEngine;

public class UIToolEventTier3 : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;

    public void OnClick()
    {
        string config = "";

        switch (dropDown.value)
        {
            case 0:
                config = "a";
                break;
            case 1:
                config = "b";
                break;
            case 2:
                config = "c";
                break;
        }

        ChangeRemoteConfig(config);
    }

    private void ChangeRemoteConfig(string config)
    {
#if DEVELOPMENT && !UNITY_EDITOR
        // DataManager.Save.RemoteConfig.EventTier3 = config;
        // DataManager.Save.Save();
#endif
        TutorialManager.S?.InitData();

        var map = DataManager.Save.Map;
        GameManager.S.KillAll();

        GameManager.S.Mode = GameMode.Normal;
        MonoScene.S.Load($"Zone{map.Zone}_{map.Level}");
    }
}
