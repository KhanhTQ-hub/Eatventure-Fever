using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemEquipmentFarm : UIItem
{
    [SerializeField] private string rsIcon = "RandomEquipment";
    private EquipmentData item;

    public override void Set(object data)
    {
        var tuple = (Tuple<ILootData, string>)data;

        item = tuple.Item1 as EquipmentData;
        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item!.Rarity.ToString());
        txtAmount.text = item.Value.Show();
    }

    public override void ShowUIInfor()
    {
        if (item.Id == -1) MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Equipment, item.Rarity));
        else MainGameScene.S.Show<UIInforEquipment>(new Tuple<int, Action, bool, bool, int, bool>(item.Id, null, false, true, 401, false));
    }
}