using System;
using System.Globalization;
using com.unimob.services.remote_config;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
#if DEVELOPMENT || UNITY_EDITOR || STAGING
using NPS.Pattern.Observer;
#endif

public class UIButtonChangeEvent : MonoBehaviour
{
#if DEVELOPMENT || UNITY_EDITOR || STAGING

    [SerializeField] private GameObject content;

    [SerializeField] private GameObject objCollect;
    [SerializeField] private GameObject objABTest;

    private void Awake()
    {
        this.RegisterListener(EventID.StartGameSuccess, StartGameSuccess);

        Dropdown();
    }

    private void StartGameSuccess(object obj)
    {
        TutorialManager.S?.RegisterComplete(94, () => { content.SetActive(true); });

        content.SetActive(DataManager.Save.Tutorial.Complete.Contains(94));
        ShowDate();
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.StartGameSuccess, StartGameSuccess);
    }

    public void OnClick()
    {
        objCollect.SetActive(!objCollect.activeSelf);
        objABTest.SetActive(!objABTest.activeSelf);
    }

    public void ClickABTest(string version)
    {

       // var _settings = RemoteConfigServiceSettings.Instance;
       // foreach (var config in _settings.Configs)
       // {
       //     if (config.Name.Contains(RemoteConfigKey.event_tier3))
       //     {
       //         config.DefaultValue = "b";
       //     }
       // }
       //  
       //  remote.Save();

        DataManager.Save.Fix();
        DataManager.Save.Save();
        TutorialManager.S?.InitData();

        var mapSave = DataManager.Save.Map;
        GameManager.S.KillAll();

        GameManager.S.Mode = GameMode.Normal;
        MonoScene.S.Load($"Zone{mapSave.Zone}_{mapSave.Level}");
    }

    [SerializeField] private TMP_Dropdown days;
    [SerializeField] private TMP_Dropdown hours;
    [SerializeField] private TMP_Dropdown minutes;
    [SerializeField] private TMP_Dropdown seconds;

    [SerializeField] private TextMeshProUGUI beginDateUtc;
    [SerializeField] private TextMeshProUGUI expiredDateUtc;

    private void ShowDate()
    {
        beginDateUtc.text = $"Begin: {EventLoop.S.BeginDateUtc.ToString(CultureInfo.InvariantCulture)}";
        expiredDateUtc.text = $"Expired: {EventLoop.S.ExpiredDateUtc.ToString(CultureInfo.InvariantCulture)}";
    }

    private void Dropdown()
    {
        days.options.Clear();
        hours.options.Clear();
        minutes.options.Clear();
        seconds.options.Clear();

        for (var i = 0; i < 60; i++)
        {
            days.options.Add(new TMP_Dropdown.OptionData()
                { text = i.ToString() });
            hours.options.Add(new TMP_Dropdown.OptionData()
                { text = i.ToString() });
            minutes.options.Add(new TMP_Dropdown.OptionData()
                { text = i.ToString() });
            seconds.options.Add(new TMP_Dropdown.OptionData()
                { text = i.ToString() });
        }
    }

    public void NowAddDays()
    {
        DateTimeStatic.NowAddDays(int.Parse(days.options[days.value].text));
    }

    public void NowAddHours()
    {
        DateTimeStatic.NowAddHours(int.Parse(days.options[hours.value].text));
    }

    public void NowAddMinutes()
    {
        DateTimeStatic.NowAddMinutes(int.Parse(days.options[minutes.value].text));
    }

    public void NowAddSeconds()
    {
        DateTimeStatic.NowAddSeconds(int.Parse(days.options[seconds.value].text));
    }
#endif
}