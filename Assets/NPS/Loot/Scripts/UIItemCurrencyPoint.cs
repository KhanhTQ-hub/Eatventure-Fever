using System;
using kt.job.math;
using NPS.Loot;
using UnityEngine;

public class UIItemCurrencyPoint : UIItem
{
	[SerializeField] protected string rsIcon = "Currency";

	private CurrencyData _item;
	private CurrencyType _type;

	public override void Set(object data)
	{
		var tuple = (Tuple<ILootData, bool>)data;

		_item = tuple.Item1 as CurrencyData;
		var eventSpecial = tuple.Item2;

		string des = "";

		if (_item.Type != CurrencyType.Point && _item.Type != CurrencyType.Point2)
		{
			des = _item.Type.ToString();
			_type = _item.Type;
		}
		else
		{
			des = !eventSpecial ? DataManager.Save.RemoteConfig.Point.ToString() : DataManager.Save.RemoteConfig.PointSpecial.ToString();
			_type = !eventSpecial ? CurrencyType.Point : CurrencyType.Point2;
		}

		imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, des);
		txtAmount.text = $"x{_item.Value.Show()}";
	}

	public override void ShowUIInfor()
	{
		MainGameScene.S.Show<UIGiftInfomation>(new Tuple<LootType, object>(LootType.Currency, _type));
	}
}
