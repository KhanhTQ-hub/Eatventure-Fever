using kt.job.math;
using NPS.Loot;

public class UIItemClothes : UIItem
{
	private ClothesData item;

	public override void Set(object data)
	{
		item = data as ClothesData;

		if (item.Id == -1)
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
		MainGameScene.S.Show<UIInfoClothes>(item.Id);
	}

	private void Show(ClothesData item)
	{
		var type = item.Id;
		var rsIcon = "Clothes";
		if (imgIcon)
		{
			imgIcon.sprite = ResourceManager.S.LoadSprite(rsIcon, type.ToString());
		}
	}

	private void ShowRandom(ClothesData item)
	{
		var rsIcon = "RandomDish";
		if (imgIconRandom)
		{
			imgIconRandom.sprite = ResourceManager.S.LoadSprite(rsIcon, item.Rarity.ToString());
		}
	}
}
