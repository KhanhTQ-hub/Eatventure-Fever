using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemChestFarm : UIItem
{
    [SerializeField] private string rsIcon = "IconChest";

    public override void Set(object data)
    {
        var tuple = (Tuple<ILootData, string>)data;
        
        var item = tuple.Item1 as ChestData;
        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Type.ToString());
        txtAmount.text = item.Value.Show();
    }

    public override void ShowUIInfor()
    {
        
    }
}
