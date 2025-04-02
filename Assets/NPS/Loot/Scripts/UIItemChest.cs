using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemChest : UIItem
{
    [SerializeField] private string rsIcon = "IconChest";

    public override void Set(object data)
    {
        var item = data as ChestData;

        imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item!.Type.ToString());
        txtAmount.text = $"x{item.Value.Show()}"; 
    }

    public override void ShowUIInfor()
    {
        
    }
}
