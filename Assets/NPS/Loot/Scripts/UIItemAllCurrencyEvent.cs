using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;
using UnityEngine.UI;

public class UIItemAllCurrencyEvent : UIItem
{
    [SerializeField] private string rsIcon = "Currency";
    [SerializeField] private Image icon;

    public override void Set(object data)
    {
        var (item1, currencyMachine) = (Tuple<ILootData, double>)data;
        var item = item1 as CurrencyData;

        var value = item.Value;
        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, Utils.GetCurrency(item.Type));

        if (Constant.CurrencyCoin.Contains(item.Type))
        {
            icon.gameObject.SetActive(true);
            icon.sprite = ResourceManager.S.LoadSprite("IconCurrency", Utils.GetCurrency(item.Type));
            var val = currencyMachine == -1 ? value : DataManager.Live.GameConfig.BaseCoin + value * currencyMachine;
            txtAmount.text = $"x{(val).Show()}";
        }
        else
        {
            txtAmount.text = $"x{value.Show()}";
            icon.gameObject.SetActive(false);
        }
    }

    public override void SetShaking(bool shaking)
    {
        if (anim) anim.SetBool("Shaking", shaking);
    }

    public override void ShowUIInfor()
    {
        
    }
}