using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemCurrency : UIItem
{
    [SerializeField] protected string rsIcon = "Currency";

    private static readonly int Shaking = Animator.StringToHash("Shaking");
    protected CurrencyData item;

    public override void Set(object data)
    {
        item = data as CurrencyData;

        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Type.ToString());
        txtAmount.text = xAmount ? $"x{item.Value.Show()}" : $"{item.Value.Show()}";
    }

    public override void SetShaking(bool shaking)
    {
        if (anim) anim.SetBool(Shaking, shaking);
    }

    public override void ShowUIInfor()
    {
        MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Currency, item.Type));
    }

    public override void SetAmountNoX()
    {
        txtAmount.text = $"{item.Value.Show()}";
    }
}