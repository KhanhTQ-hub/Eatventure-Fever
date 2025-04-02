using System;
using kt.job.math;
using NPS.Loot;

public class UIItemDish : UIItem
{
    private string rsFrame = "MiniFrameEquipment";
    private DishCrData item;
    public override void Set(object data)
    {
        item = data as DishCrData;

        if (imgFrame) imgFrame.sprite = ResourceManager.S.LoadSprite(rsFrame, item.Rarity.ToString());
        
        if(item.Id == -1)
        {
            ShowRandom(item);
            IsRandom(true);
        }
        else
        {
            Show(item);
            IsRandom();
        }
        
        txtAmount.text = $"x{item.Value.Show()}";
    }

    public override void ShowUIInfor()
    {
        if (item.Id == -1) MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Dish, item.Rarity));
        else MainGameScene.S.Show<UIInforDish>(item.Id);
    }

    public void Show(DishCrData item)
    {
        var type = (DishType)item.Id;
        string rsIcon = "Upgrade";
        if(imgIcon) imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, type.ToString());
    }

    public void ShowRandom(DishCrData item)
    {
        string rsIcon = "RandomDish";
        if(imgIconRandom) imgIconRandom.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Rarity.ToString());
    }
}