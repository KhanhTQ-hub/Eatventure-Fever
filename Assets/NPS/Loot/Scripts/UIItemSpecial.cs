using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemSpecial : UIItem
{
    [SerializeField] private bool rsUseHead = false;
    [SerializeField] private bool rarity2color = false;
    
    private string rsFrame = "MiniFrameEquipment";

    private SpecialRwData item;
    private Rarity rarity;

    public override void Set(object data)
    {
        item = data as SpecialRwData;

        txtAmount.text = xAmount ? $"x{item.Value.Show()}" : $"{item.Value.Show()}";
        
       if (item.Id == -1)
        {
            ShowRandom(item);
            IsRandom(true);
        }
        else
        {
            Show(item);
            IsRandom();
        }
       
        if (imgFrame) imgFrame.sprite = ResourceManager.S.LoadSprite(rsFrame, rarity.ToString());
    }

    public override void ShowUIInfor()
    {
        if (item.Id == -1) MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Special, item.Rarity));
        else
        {
            MainGameScene.S.Show<UIIntroduceSpecial>(new Tuple<int, int, int, Action, bool>(item.Id, 0, 0, (() => AudioManager.S.PlayOneShoot("Btn_Click")), true));
            MainGameScene.S.View<UIIntroduceSpecial>().HideReceived();
        }
    }

    public void OnClick()
    {
        var save = DataManager.Save.Special.Dictionary[item.Id];
        MainGameScene.S.Show<UIIntroduceSpecial>(new Tuple<int, int, Action, bool>(save.Special, save.Star, null, true));
    }

    private void Show(SpecialRwData item)
    {
        rarity = DataManager.Base.Special.Dictionary[item.Id].Rarity;
        var rsIcon = rsUseHead ? "IconSpecial" : (GameManager.S.Mode == GameMode.Normal ? "Special" : $"Special_{Utils.GetCurrency()}");
      
        if (imgIcon) imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Id.ToString());

        if (rarity2color)
            txtAmount.color = Constant.Rarity2Color2[rarity];
    }

    private void ShowRandom(SpecialRwData item)
    {
        rarity = item.Rarity;
        var rsIcon = "RandomSpecial";
        if(imgIconRandom) imgIconRandom.sprite = ResourceManager.S.LoadSprite(rsIcon, rarity.ToString());
    }
}