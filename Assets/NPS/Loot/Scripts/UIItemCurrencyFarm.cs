using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemCurrencyFarm : UIItem
{
    [SerializeField] private string rsIcon = "Currency";
    private CurrencyData item;
    public override void Set(object data)
    {
        var tuple = (Tuple<ILootData, string>)data;
        
        item = tuple.Item1 as CurrencyData;
        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Type.ToString());
        txtAmount.text = item.Value.Show();
    }

    public override void ShowUIInfor()
    {
        MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Currency, item.Type));
    }
}
