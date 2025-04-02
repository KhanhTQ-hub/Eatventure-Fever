using System;
using System.Collections.Generic;
using UnityEngine;

#if DEVELOPMENT || UNITY_EDITOR || STAGING
using TMPro;
using NPS.Loot;
using System.Linq;
using NPS.Pattern.Observer;
using com.unimob.mec;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
#endif

public class UIMainTest : MonoBehaviour
{
    [SerializeField] private GameObject content;

    private bool isGiftBox = false;

    private void Awake()
    {
        content.SetActive(false);

#if DEVELOPMENT || UNITY_EDITOR || STAGING
        if (!S) S = this;

        Dropdown();
        ShowTool();
        foreach (var item in autos)
        {
            item.Set();
        }

        evtGiftBox?.AddListener(OnGiftBox);
        evtOpenDish?.AddListener(OnOpenDish);
        evtUpgradeItemSuccess?.AddListener(OnUpgradeItem);
        this.RegisterListener(EventID.MaxUpdateMachine, OnMaxUpdateMachine);

#if DEVELOPMENT || UNITY_EDITOR
        content.SetActive(true);
#endif
#endif
    }

    private void OnGiftBox(GiftBox obj)
    {
        if (isGiftBox)
            obj.Open();
    }

#if DEVELOPMENT || UNITY_EDITOR || STAGING
    public static UIMainTest S;

    [Header("Human")] [SerializeField] private DictionaryHuman humans;

    [Header("Dropdown")] [SerializeField] private TMP_Dropdown ddSpecial;
    [SerializeField] private TMP_Dropdown ddMap;
    [SerializeField] private TMP_Dropdown abTest;
    [SerializeField] private TMP_Dropdown ddNoelMap;

    [Header("Auto")] [SerializeField] private List<AutoTestItem> autos;
    [SerializeField] private Transform autoPos;

#if UNITY_EDITOR
    private void OnValidate()
    {
        autos.Clear();
        foreach (Transform item in autoPos)
        {
            var auto = item.GetComponent<AutoTestItem>();
            if (auto)
                autos.Add(auto);
        }
    }
#endif

    private void Dropdown()
    {
        ddSpecial.options.Clear();
        foreach (var item in DataManager.Base.Special.Dictionary)
        {
            ddSpecial.options.Add(new TMP_Dropdown.OptionData()
                { text = I2.Loc.LocalizationManager.GetTranslation($"Name_Special/{item.Key}") });
        }

        var mapSave = DataManager.Save.Map;

        ddMap.options.Clear();
        foreach (var item in DataManager.Base.Zone.Dictionary)
        {
            if (item.Value.Zone < mapSave.Zone) continue;
            if (item.Value.Zone >= 100) continue;

            foreach (var entity in item.Value.Dictionary)
            {
                if (item.Value.Zone == mapSave.Zone && entity.Key <= mapSave.Level) continue;

                ddMap.options.Add(new TMP_Dropdown.OptionData()
                    { text = $"{item.Value.Zone}-{entity.Key}" });
            }
        }

        abTest.options.Clear();
        for (var i = 0; i < 3; i++)
        {
            abTest.options.Add(new TMP_Dropdown.OptionData()
                { text = i.ToString() });
        }


        var universeSave = DataManager.Save.Universe;
        ddNoelMap.options.Clear();
        foreach (var item in DataManager.Base.Zone.Dictionary)
        {
            if (item.Value.Zone != 500) continue;

            foreach (var entity in item.Value.Dictionary)
            {
                if (entity.Key <= universeSave.Level) continue;

                ddNoelMap.options.Add(new TMP_Dropdown.OptionData()
                    { text = $"{item.Value.Zone}-{entity.Key}" });
            }
        }
    }

    public void Toggle()
    {
        content.SetActive(!content.activeSelf);
    }

    public void LootSpecial()
    {
        var loots = new List<LootData>
        {
            new LootData()
            {
                Type = LootType.Special,
                Data = new SpecialRwData()
                {
                    Id = ddSpecial.value,
                    Value = 1
                }
            }
        };

        MainGameScene.S.Loot.Loot(loots, Location.Cheat, LocationId.Empty);
    }

    public void AllSpecial()
    {
        var special = DataManager.Save.Special;
        foreach (var item in special.Dictionary.Keys.ToList().Where(item => special.Dictionary[item].Level == -1))
        {
            special.Dictionary[item].Level = 0;
            special.Dictionary[item].Count = 0;
        }
    }

    public void AllEquipment()
    {
        var special = DataManager.Save.Collection;
        foreach (var item in special.Equipment.Keys.ToList())
        {
            special.Equipment[item].IncreaseCount(100);
        }
    }

    public void ChangeMap()
    {
        if (ddMap.options.Count <= 0) return;

        var str = ddMap.options[ddMap.value].text.Split('-');

        var mapSave = DataManager.Save.Map;

        mapSave.Zone = int.Parse(str[0]);
        mapSave.Level = int.Parse(str[1]);

        DataManager.Save.NextMap();

        MonoScene.S.LoadAsync($"Zone{mapSave.Zone}_{mapSave.Level}");

        GameManager.S?.KillAll();
        Time.timeScale = 0;

        if (mapSave.Level == 0)
        {
            MainGameScene.S.Show<UILoadingMap>(new Tuple<int, Action>(mapSave.Zone, () =>
            {
                GameManager.S.Mode = GameMode.Normal;

                DataManager.Save.UpdateGameMode();
                Time.timeScale = 1;
                MonoScene.S.Active();
            }));
        }
        else
        {
            MainGameScene.S.Show<UILoading>();
            MainGameScene.S.View<UILoading>().Loading(3f, () =>
            {
                GameManager.S.Mode = GameMode.Normal;

                DataManager.Save.UpdateGameMode();
                Time.timeScale = 1;
                MonoScene.S.Active();
            });
        }
    }

    public void ChangeMapNoel()
    {
        if (DataManager.Save.Universe.NextLevel())
        {
            GameManager.S?.KillAll();
            DataManager.Save.NextUniverse();

            var str = ddNoelMap.options[ddNoelMap.value].text.Split('-');

            var universeSave = DataManager.Save.Universe;

            universeSave.Zone = int.Parse(str[0]);
            universeSave.Level = int.Parse(str[1]);

            MonoScene.S.LoadAsync($"Zone{universeSave.Zone}_{universeSave.Level}");

            Time.timeScale = 0;

            if (universeSave.Level != 0)
            {
                MainGameScene.S.Show<UILoading>();
                MainGameScene.S.View<UILoading>().Loading(3f, () =>
                {
                    DataManager.Save.UpdateGameMode();
                    Time.timeScale = 1;
                    MonoScene.S.Active();
                });
            }

            AppManager.Cloud.PostUserData();
        }
    }

    public void ABtestReward()
    {
        if (abTest.options.Count <= 0) return;
        // var saveRemote = DataManager.Save.RemoteConfig;
        // saveRemote.Configs[RemoteConfigKey.ab_reward_ads] = abTest.value.ToString();
        // saveRemote.Save();
    }

    public void AllCoin()
    {
        var user = DataManager.Save.User;
        switch (GameManager.S.Mode)
        {
            case GameMode.Normal:
                user.SetCurrency(CurrencyType.Coin, Constant.Max);
                break;
            case GameMode.Universe:
                user.SetCurrency(CurrencyType.Universe, Constant.Max);
                break;
            case GameMode.UniverseSpecial:
                user.SetCurrency(CurrencyType.UniverseSpecial, Constant.Max);
                break;
        }

        user.Save();
    }

    public void ResetGame()
    {
        GameManager.S.ResetGame();
    }

    private bool isShowTool = false;

    public void Tool()
    {
        isShowTool = !isShowTool;

        ShowTool();
    }

    public void ShowToolCheat()
    {
        MainGameScene.S.Show<UIToolCheat>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Toggle();
        }
    }

    private void ShowTool()
    {
        foreach (var item in autos)
        {
            //item.gameObject.SetActive(isShowTool);
        }

        bool checkUniverseNoel = (GameManager.S.Mode != GameMode.Normal) && DataManager.Save.RemoteConfig.Event.Type == NPS.Remote.EventType.Noel_New;

        ddNoelMap.gameObject.SetActive(checkUniverseNoel);
    }

    private float timeScale = 1f;

    public void XTime(AutoTestItem button)
    {
        timeScale++;
        if (Time.timeScale >= 4f)
        {
            Time.timeScale = 1f;
            timeScale = 1f;
        }
        else
            Time.timeScale = timeScale;

        button.OnClick(Time.timeScale != 1);
    }

    [SerializeField] private CollectionPositionDinner dinners;

    public class ActionData
    {
        public ActionType Type;
        public Machines Machine;
    }

    public enum ActionType
    {
        Unlock,
        Upgrade,
        Upgrades,
        Renovate,
    }

    [SerializeField] private DictionaryMachine machines;
    [SerializeField] private GameEventBase<int> evtOpenDish;
    [SerializeField] private GameEventBase evtUpgradeItemSuccess;
    [SerializeField] private GameEventGiftBox evtGiftBox;

    private ActionData action;
    private CoroutineHandle handle;
    private bool isAuto = false;

    private void OnOpenDish(int data)
    {
        action = null;
        Auto();
    }

    private void OnUpgradeItem()
    {
        action = null;
        Auto();
    }

    private void OnMaxUpdateMachine(object obj)
    {
        action = null;
        Auto();
    }

    [Button]
    public void CheckAction(AutoTestItem button)
    {
        isAuto = !isAuto;
        button.OnClick(isAuto);

        if (!isAuto)
        {
            Timing.KillCoroutines(handle);
            handle = default;

            action = null;
        }
        else
            Auto();

        if (isAuto)
        {
            if (!isGiftBox)
                GiftBox(autos.Find(x => x.gameObject.name == "GiftBox"));

            if (!isNext)
                Next(autos.Find(x => x.gameObject.name == "Next"));
        }
    }

    private void Auto()
    {
        if (!isAuto) return;

        if (action == null)
        {
            FindAction();

            if (action != null)
            {
                AllCoin();
                Action();
            }
        }
    }

    private void FindAction()
    {
        foreach (var id in DataManager.Base.Zone.Level.Machine.Keys)
        {
            var item = machines.Get(id);

            if (!item.Save.IsOpen)
            {
                action = new ActionData()
                {
                    Type = ActionType.Unlock,
                    Machine = item,
                };

                return;
            }

            if (item.Save.Level < item.Entity.Milestone[item.Entity.Milestone.Count - 1])
            {
                action = new ActionData()
                {
                    Type = ActionType.Upgrade,
                    Machine = item,
                };

                return;
            }
        }

        if (DataManager.Save.Upgrade.Bought.Any(item => !item))
        {
            action = new ActionData()
            {
                Type = ActionType.Upgrades,
            };

            return;
        }

        if (isNext)
        {
            action = new ActionData()
            {
                Type = ActionType.Renovate,
            };
        }
    }

    [Button]
    private void Action()
    {
        if (handle.IsRunning) return;

        if (action.Machine)
        {
            Machines machine = machines.Get(action.Machine.Data.Dish.Id);
            if (machine)
                ConfigCamera.S.MoveY(machine.transform.position.y, () => { handle = Timing.RunCoroutine(_Action()); });
        }
        else
        {
            handle = Timing.RunCoroutine(_Action());
        }
    }

    private IEnumerator<float> _Action()
    {
        this.PostEvent(EventID.HideLeanUI);

        switch (action.Type)
        {
            case ActionType.Unlock:
                action.Machine.OnClick();

                yield return Timing.WaitForSeconds(0.75f);

                Kill();
                MainGameScene.S.Unlock.Unlock();
                break;
            case ActionType.Upgrade:
                action.Machine.OnClick();

                yield return Timing.WaitForSeconds(0.75f);

                Kill();
                MainGameScene.S.Upgrade.Upgrade();
                break;
            case ActionType.Upgrades:
                var view = MainGameScene.S.View<UIUpgrades>();
                if (!view.IsShow)
                {
                    view.Show();
                    yield return Timing.WaitForSeconds(1.5f);
                }

                while (true)
                {
                    if (view.Dsc.isInt && view.Dsc.isIntComplete)
                        break;

                    yield return Timing.WaitForOneFrame;
                }

                yield return Timing.WaitForSeconds(0.1f);

                UIUpgradeItem item = null;
                foreach (var active in view.Dsc.GetActiveItems())
                {
                    var it = active.GetComponent<UIUpgradeItem>();
                    if (!it) continue;
                    if (!item || (item && item.Index > it.Index))
                    {
                        item = it;
                    }
                }

                Kill();
                if (item) item.Buy();
                break;
            case ActionType.Renovate:
                var viewR = MainGameScene.S.View<UIRenovate>();
                yield return Timing.WaitForSeconds(0.75f);
                Kill();
                //viewR.NextLevel();
                break;
        }
    }

    private void Kill()
    {
        Timing.KillCoroutines(handle);
        handle = default;
    }

    public void GiftBox(AutoTestItem button)
    {
        isGiftBox = !isGiftBox;

        button.OnClick(isGiftBox);
    }

    private bool isNext = false;

    public void Next(AutoTestItem button)
    {
        isNext = !isNext;

        button.OnClick(isNext);
    }

    private void OnDestroy()
    {
        evtGiftBox?.RemoveListener(OnGiftBox);
        evtOpenDish?.RemoveListener(OnOpenDish);
        evtUpgradeItemSuccess?.RemoveListener(OnUpgradeItem);
        this.RemoveListener(EventID.MaxUpdateMachine, OnMaxUpdateMachine);
    }
#endif
}