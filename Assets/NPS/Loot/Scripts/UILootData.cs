using System;
using System.Collections.Generic;
using NPS.Loot;

public class UILootData
{
    public List<LootData> Rewards { private set; get; }
    public Action HideCallback { private set; get; }
    public bool AllCurrencyMachine { private set; get; }
    public string Location { private set; get; }
    public string LocationId { private set; get; }

    public bool ShowInfor { private set; get; }

    public UILootData(List<LootData> rewards, string location, string locationId, Action hideCallback, bool allCurrencyMachine, bool showInfor)
    {
        Rewards = rewards;
        HideCallback = hideCallback;
        AllCurrencyMachine = allCurrencyMachine;
        Location = location;
        LocationId = locationId;
        ShowInfor = showInfor;
    }
}