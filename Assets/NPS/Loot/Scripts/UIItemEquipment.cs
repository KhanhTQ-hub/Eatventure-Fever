using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemEquipment : UIItem
{
    [SerializeField] private bool rarity2color = false;

    private EquipmentData item;

    private static readonly int Shaking = Animator.StringToHash("Shaking");
    private Rarity rarity;

    public override void Set(object data)
    {
        item = data as EquipmentData;
        
        txtAmount.text = xAmount ? $"x{item.Value.Show()}" : $"{item.Value.Show()}";
        if (item.Id == -1)
        {
            ShowRandom();
            IsRandom(true);
        }
        else
        {
            Show();
            IsRandom();
        }
        
        if (imgFrame)
        {
            imgFrame.sprite = ResourceManager.S.LoadSprite("MiniFrameEquipment", rarity.ToString());
        }
    }

    public override void SetShaking(bool shaking)
    {
        if (anim && anim.gameObject.activeSelf)
        {
            anim.SetBool(Shaking, shaking);
        }
    }

    public override void ShowUIInfor()
    {
        if (item.Id == -1)
        {
            MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Equipment, item.Rarity));
        }
        else
        {
            MainGameScene.S.Show<UIInforEquipment>(new Tuple<int, Action, bool, bool, int, bool>(item.Id, null, false, true, 401, false));
            MainGameScene.S.View<UIInforEquipment>().HideReceived();
        }
    }

    private void Show()
    {
        var moonAb = GameManager.S.Mode != GameMode.Normal;
        rarity = DataManager.Base.ZoneUniverse.LevelMode.Equipment[item.Id].Rarity;

        var equip = moonAb ? $"Equipment_{Utils.GetCurrency()}" : "Equipment";
        
        if (imgIcon) imgIcon.sprite = ResourceManager.S.LoadSprite(equip, item.Id.ToString());
        
        if (rarity2color)
            txtAmount.color = Constant.Rarity2Color2[rarity];
    }

    private void ShowRandom()
    {
        rarity = item!.Rarity;
        string rsIcon = "RandomEquipment";
        if (imgIconRandom) imgIconRandom.sprite = ResourceManager.S.LoadSprite(rsIcon, rarity.ToString());
    }
}