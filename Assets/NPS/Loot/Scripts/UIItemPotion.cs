using System;
using kt.job.math;
using NPS.Loot;

public class UIItemPotion : UIItem
{
    private PotionData item;
    public override void Set(object data)
    {
        item = data as PotionData;
        imgIcon.sprite = ResourceManager.S.LoadSprite("Potion", item.Type.ToString());
        txtAmount.text = $"x{item.Value.Show()}";
    }

    public override void ShowUIInfor()
    {
        MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Potion, item.Type));
    }
}