using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemKey : UIItem
{
    private KeyData item;
    private static readonly int Shaking = Animator.StringToHash("Shaking");
    public override void Set(object data)
    {
        item = data as KeyData;

        imgIcon.sprite = ResourceManager.S.LoadSprite("ChestKey", item.Type.ToString());
        imgIcon.SetNativeSize();
        txtAmount.text = xAmount ? $"x{item.Value.Show()}" : $"{item.Value.Show()}";
    }

    public override void ShowUIInfor()
    {
        MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Key, item.Type));
    }

    public override void SetAmountNoX()
    {
        txtAmount.text = $"{item.Value.Show()}";
    }    
    
    public override void SetShaking(bool shaking)
    {
        if (anim && anim.gameObject.activeSelf)
        {
            anim.SetBool(Shaking, shaking);
        }
    }
}