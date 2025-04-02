using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemCurrencyDaily : UIItem
{
    [SerializeField] private string rsIcon = "Currency";

    public override void Set(object data)
    {
        var item = data as CurrencyData;

        if (item.Type == CurrencyType.Ticket)
        {
            var value = item.Value > 3 ? 3 : item.Value;
            imgIcon.sprite = ResourceManager.S.LoadSprite("Ticket", value.ToString());
            imgIcon.SetNativeSize();
        }
        else
            imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Type.ToString());
        
        txtAmount.text = $"x{item.Value.Show()}";
    }

    public override void ShowUIInfor()
    {
        
    }
}
