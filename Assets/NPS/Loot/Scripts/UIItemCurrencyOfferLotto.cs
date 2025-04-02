using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemCurrencyOfferLotto : UIItem
{
    [SerializeField] private string rsIcon = "Currency";
    private CurrencyData item;
    public override void Set(object data)
    {
        item = data as CurrencyData;

        if (item!.Type == CurrencyType.Ticket)
        {
            imgIcon.sprite = ResourceManager.S.LoadSprite("Ticket", "1");
            imgIcon.SetNativeSize();
            imgIcon.transform.localScale = new Vector3(1.25f, 1.25f, 1);
        }
        else
        {
            imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Type.ToString());
            imgIcon.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
        
        txtAmount.text = $"x{item.Value.Show()}";
    }

    public override void ShowUIInfor()
    {
        MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Currency, item.Type));
    }
}
