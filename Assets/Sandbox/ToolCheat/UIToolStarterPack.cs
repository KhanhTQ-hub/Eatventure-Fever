using TMPro;
using UnityEngine;

public class UIToolStarterPack : MonoBehaviour
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
        //DataManager.Save.Rookie.TestAB(config);
        DataManager.Save.Save();
        TutorialManager.S?.InitData();

        var saveMap = DataManager.Save.Map;
        GameManager.S.KillAll();

        GameManager.S.Mode = GameMode.Normal;
        MonoScene.S.Load($"Zone{saveMap.Zone}_{saveMap.Level}");
    }
}
