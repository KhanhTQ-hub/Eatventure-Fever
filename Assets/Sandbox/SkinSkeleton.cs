using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkinSkeleton : MonoBehaviour
{
	public static SkeletonGraphic SpawnSkin(int id, Transform pos)
	{
		var sg = PoolManager.S.Spawn(ResourceManager.S.LoadGraphic($"Special/{id}"), pos);
		sg.Initialize(false);

		sg.Skeleton.SetSkin(id == 5 ? $"{Utils.GetCurrency()}_Normal" : Utils.GetCurrency());
		sg.Skeleton.SetSlotsToSetupPose();
		sg.LateUpdate();

		return sg;
	}

	public static SkeletonGraphic SpawnSkinClothesGraphic(HumanType type, int clothes, Transform pos)
	{
		var _type = type == HumanType.Waiter ? "Staff" : type.ToString();
		
		var sg = PoolManager.S.Spawn(ResourceManager.S.LoadGraphic($"{_type}/{clothes}"), pos);
		var skeleton = sg.Skeleton;
		var skeletonData = skeleton.Data;
		var mixAndMatchSkin = new Skin("custom");
		mixAndMatchSkin.AddSkin(skeletonData.FindSkin(1 + "/Body"));
		mixAndMatchSkin.AddSkin(skeletonData.FindSkin(1 + "/Face"));
		mixAndMatchSkin.AddSkin(skeletonData.FindSkin(1 + "/Hat"));
		skeleton.SetSkin(mixAndMatchSkin);
		skeleton.SetSlotsToSetupPose();
		
		sg?.Initialize(false);

		return sg;
	}

	public static SkeletonAnimation SpawnSkinClothes(HumanType type, int clothes, Transform pos)
	{
		var _type = type == HumanType.Waiter ? "Staff" : type.ToString();
		var sg = PoolManager.S.Spawn(ResourceManager.S.LoadAnimation($"{_type}/{clothes}"), pos);
		sg?.Initialize(false);

		return sg;
	}
}
