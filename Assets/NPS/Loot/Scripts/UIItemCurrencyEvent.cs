using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCurrencyEvent : UIItem
{
    [SerializeField] private string rsIcon = "Currency";
    [SerializeField] private Image icon;
    
    private static readonly int Shaking = Animator.StringToHash("Shaking");
    private CurrencyData item;

    public override void Set(object data)
    {
        item = data as CurrencyData;

        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, Utils.GetCurrency(item!.Type));

        if (Constant.CurrencyCoin.Contains(item.Type))
        {
            icon.gameObject.SetActive(true);
            icon.sprite = ResourceManager.S.LoadSprite("IconCurrency", Utils.GetCurrency(item.Type));
        }
        else icon.gameObject.SetActive(false);

        txtAmount.text = $"x{item.Value.Show()}";
    }

    public override void SetShaking(bool shaking)
    {
        if (anim) anim.SetBool(Shaking, shaking);
    }

    public override void ShowUIInfor()
    {
        if (!Constant.CurrencyCoin.Contains(item.Type))
        {
            MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Currency ,item.Type));
        }
    }
}