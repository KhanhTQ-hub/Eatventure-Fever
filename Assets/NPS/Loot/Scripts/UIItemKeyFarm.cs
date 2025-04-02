using System;
using kt.job.math;
using NPS.Loot;

public class UIItemKeyFarm : UIItem
{
    private KeyData item;
    public override void Set(object data)
    {
        var tuple = (Tuple<ILootData, string>)data;

        item = tuple.Item1 as KeyData;
        imgIcon.sprite = ResourceManager.S.LoadSprite("ChestKey", item!.Type.ToString());
        imgIcon.SetNativeSize();
        txtAmount.text = item.Value.Show();
    }

    public override void ShowUIInfor()
    {
        MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Key, item.Type));
    }
}