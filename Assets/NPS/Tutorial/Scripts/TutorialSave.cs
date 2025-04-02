using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using kt.job.math;
using NPS.Pattern.Observer;
using UnityEngine;
using EventType = NPS.Remote.EventType;

[System.Serializable]
public class TutorialSave : BaseDataSave, INextMap
{
	public int CurTut = 0;
	public int CurStep = 0;

	public List<int> Complete = new List<int>();

	public override void Fix()
	{
		base.Fix();

		CurTut = 0;
		CurStep = 0;

		Complete ??= new List<int>();

		var zone = DataManager.Save.Map.Zone;
		var level = DataManager.Save.Map.Level;

		{
			for (var j = 61; j <= 62; j++)
			{
				if (!Complete.Contains(j))
					Complete.Add(j);
			}

			if (!Complete.Contains(7))
				Complete.Add(7);
		}

		if (zone == 0)
		{
			if (level > 0)
			{
				for (var i = 1; i <= 5; i++)
				{
					if (!Complete.Contains(i))
						Complete.Add(i);
				}

				if (!Complete.Contains(31))
					Complete.Add(31);
				if (!Complete.Contains(32))
					Complete.Add(32);
				if (!Complete.Contains(41))
					Complete.Add(41);
				if (!Complete.Contains(42))
					Complete.Add(42);
			}

			if (level > 1)
			{
				for (var i = 1; i <= 8; i++)
				{
					if (i == 6)
					{
						for (var j = 61; j <= 62; j++)
						{
							if (!Complete.Contains(j))
								Complete.Add(j);
						}

						continue;
					}

					if (!Complete.Contains(i))
						Complete.Add(i);
				}

				if (!Complete.Contains(611))
					Complete.Add(611);

				if (!Complete.Contains(11))
					Complete.Add(11);

				if (!Complete.Contains(15))
				{
					Complete.Add(15);
					DataManager.Save.Rookie.Open();
				}
				
				if (!Complete.Contains(33))
					Complete.Add(33);
			}

			if (level > 2)
			{
				if (!Complete.Contains(10))
					Complete.Add(10);
				if (!Complete.Contains(101))
					Complete.Add(101);
				if (!Complete.Contains(102))
					Complete.Add(102);

				if (!Complete.Contains(12))
					Complete.Add(12);
				if (!Complete.Contains(19))
					Complete.Add(19);
				if (!Complete.Contains(91))
					Complete.Add(91);
				if (!Complete.Contains(92))
					Complete.Add(92);
				if (!Complete.Contains(93))
					Complete.Add(93);
				if (!Complete.Contains(94))
					Complete.Add(94);
			}

			if (level > 3)
			{
				if (!Complete.Contains(13))
					Complete.Add(13);
				for (var j = 131; j <= 132; j++)
				{
					if (!Complete.Contains(j))
						Complete.Add(j);
				}

				if (!Complete.Contains(14))
					Complete.Add(14);
				if (!Complete.Contains(133))
					Complete.Add(133);
				if (!Complete.Contains(95))
					Complete.Add(95);
			}

			if (level > 4)
			{
				if (!Complete.Contains(16))
					Complete.Add(16);
				if (!Complete.Contains(161))
					Complete.Add(161);
			}
		}

		if (zone == 1)
		{
			if (level > 0)
			{
				if (!Complete.Contains(20))
					Complete.Add(20);
			}
		}

		var tutZone0 = new List<int>()
		{
			1, 2, 3, 4, 41, 42, 5, 6, 7, 8, 9, 91, 10, 11, 12, 13, 14, 15, 16, 19, 91, 92, 93, 94, 95,
			31, 61, 611, 62, 31, 101, 102, 131, 132, 161, 111, 133, 33, 34, 35
		};

		if (zone > 0)
		{
			if (!Complete.Contains(15))
			{
				Complete.Add(15);
				DataManager.Save.Rookie.Open();
			}

			foreach (var item in tutZone0.Where(item => !Complete.Contains(item)))
			{
				Complete.Add(item);
			}
		}

		if (zone > 1)
		{
			if (!Complete.Contains(20))
				Complete.Add(20);
		}

		var hollysave = DataManager.Save.HollyJolly;
		var listMile = hollysave.ListMilestone;
		if (!Complete.Contains(27) && listMile != null && listMile.Count != 0)
		{
			if (listMile[0].IsReceived)
				Complete.Add(27);
		}
	}

	private void FixTutorialEventPoint()
	{
		var remoteConfig = DataManager.Save.RemoteConfig;

		if (remoteConfig.CheckEnableEvent(EventType.Easter))
		{
			var easterSave = DataManager.Save.Easter;
			var listEaster = easterSave.ListMilestone;
			var tut = remoteConfig.PointSpecial == Point.Easter ? 281 : 28;
			if (!Complete.Contains(tut) && listEaster != null && listEaster.Count != 0)
			{
				if (listEaster[0].IsReceived)
					Complete.Add(tut);
			}

			var tutMini = remoteConfig.PointSpecial == Point.Easter ? 291 : 29;
			if (DataManager.Save.MiniGameEaster.Dictionary.Any(item => item.Value.Type == LandPlotType.Open))
			{
				if (!Complete.Contains(tutMini))
					Complete.Add(tutMini);
			}
		}

		if (remoteConfig.CheckEnableEvent(EventType.HelloSummer))
		{
			var helloSummerSave = DataManager.Save.HelloSummer;
			var listHellosummer = helloSummerSave.ListMilestone;
			var tut = remoteConfig.PointSpecial == Point.HelloSummer ? 281 : 28;
			if (!Complete.Contains(tut) && listHellosummer != null && listHellosummer.Count != 0)
			{
				if (listHellosummer[0].IsReceived)
					Complete.Add(tut);
			}

			var tutMini = remoteConfig.PointSpecial == Point.HelloSummer ? 291 : 29;
			if (DataManager.Save.MiniGameHelloSummer.Dictionary.Any(item => item.Value.Type == LandPlotType.Open))
			{
				if (!Complete.Contains(tutMini))
					Complete.Add(tutMini);
			}
		}
		
		if (remoteConfig.CheckEnableEvent(EventType.HollyJolly))
		{
			var hollyJollySave = DataManager.Save.HollyJolly;
			var list = hollyJollySave.ListMilestone;
			var tut = remoteConfig.PointSpecial == Point.HollyJolly ? 281 : 28;
			if (!Complete.Contains(tut) && list != null && list.Count != 0)
			{
				if (list[0].IsReceived)
					Complete.Add(tut);
			}

			var tutMini = remoteConfig.PointSpecial == Point.HollyJolly ? 291 : 29;
			if (DataManager.Save.MiniGameHollyJolly.Dictionary.Any(item => item.Value.Type == LandPlotType.Open))
			{
				if (!Complete.Contains(tutMini))
					Complete.Add(tutMini);
			}
		}
		
		if (remoteConfig.CheckEnableEvent(EventType.Autumn))
		{
			var save = DataManager.Save.Autumn;
			var list = save.ListMilestone;
			var tut = remoteConfig.PointSpecial == Point.Autumn ? 281 : 28;
			if (!Complete.Contains(tut) && list != null && list.Count != 0)
			{
				if (list[0].IsReceived)
					Complete.Add(tut);
			}

			var tutMini = remoteConfig.PointSpecial == Point.Autumn ? 291 : 29;
			if (DataManager.Save.MiniGameAutumn.Dictionary.Any(item => item.Value.Type == LandPlotType.Open))
			{
				if (!Complete.Contains(tutMini))
					Complete.Add(tutMini);
			}
		}
		
		if (remoteConfig.CheckEnableEvent(EventType.ThanksGiving2))
		{
			var save = DataManager.Save.ThanksGiving2;
			var list = save.ListMilestone;
			var tut = remoteConfig.PointSpecial == Point.ThanksGiving2 ? 281 : 28;
			if (!Complete.Contains(tut) && list != null && list.Count != 0)
			{
				if (list[0].IsReceived)
					Complete.Add(tut);
			}

			var tutMini = remoteConfig.PointSpecial == Point.ThanksGiving2 ? 291 : 29;
			if (DataManager.Save.MiniGameThanksGiving2.Dictionary.Any(item => item.Value.Type == LandPlotType.Open))
			{
				if (!Complete.Contains(tutMini))
					Complete.Add(tutMini);
			}
		}
	}

	public void NextMap()
	{
		Fix();
		Save();
	}

	public void ClearUniverse()
	{
		if (Complete.Contains(23))
			Complete.Remove(23);
		if (Complete.Contains(24))
			Complete.Remove(24);
		if (Complete.Contains(25))
			Complete.Remove(25);
		if (Complete.Contains(26))
			Complete.Remove(26);
		if (Complete.Contains(27))
			Complete.Remove(27);

		NextUniverse();

		Save();
	}

	public void ClearUniverseSpecial()
	{
		if (Complete.Contains(203))
			Complete.Remove(203);
		if (Complete.Contains(204))
			Complete.Remove(204);
		if (Complete.Contains(205))
			Complete.Remove(205);
		if (Complete.Contains(206))
			Complete.Remove(206);
		if (Complete.Contains(27))
			Complete.Remove(27);

		NextUniverseSpecial();

		Save();
	}

	public void ClearTutMiniGamePoint(bool eventSpecial)
	{
		if (eventSpecial)
		{
			if (Complete.Contains(28))
				Complete.Remove(28);
			if (Complete.Contains(29))
				Complete.Remove(29);
			if (Complete.Contains(30))
				Complete.Remove(30);
		}
		else
		{
			if (Complete.Contains(281))
				Complete.Remove(281);
			if (Complete.Contains(291))
				Complete.Remove(291);
			if (Complete.Contains(301))
				Complete.Remove(301);
		}
	}

	public void NextUniverse()
	{
		if (Complete.Contains(1001))
			Complete.Remove(1001);
		if (Complete.Contains(1002))
			Complete.Remove(1002);
		if (Complete.Contains(1003))
			Complete.Remove(1003);
		if (Complete.Contains(1004))
			Complete.Remove(1004);
		if (Complete.Contains(1005))
			Complete.Remove(1005);
	}

	public void NextUniverseSpecial()
	{
		if (Complete.Contains(1011))
			Complete.Remove(1011);
		if (Complete.Contains(1012))
			Complete.Remove(1012);
		if (Complete.Contains(1013))
			Complete.Remove(1013);
		if (Complete.Contains(1014))
			Complete.Remove(1014);
		if (Complete.Contains(1015))
			Complete.Remove(1015);
	}

	public void ClearTutorialPuzzle()
	{
		if (Complete.Contains(36))
			Complete.Remove(36);
	}
	
#if UNITY_EDITOR
	[Button]
	public void Test()
	{
		// Observer.S?.PostEvent(EventID.StartTutorial, 999);
	}
#endif
}
