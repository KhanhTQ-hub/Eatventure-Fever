using System.Collections.Generic;
using System.Globalization;
using NPS.Loot;

public class PuzzleEnvelopeData : ILootData
{
	public bool IsRandom => Type.Equals(-1);
	
	public int Type; 
	public double Value;
	
	public PuzzleEnvelopeData() { }
	
	public PuzzleEnvelopeData(string content)
	{
		var str = content.Split(';');
		var s = 0;
		
		Type = int.Parse(str[s + 1]);
		Value = double.Parse(str[s + 2], CultureInfo.InvariantCulture.NumberFormat);
	}

	private void Random()
	{
		Type = DataManager.Base.Puzzle.GachaFragmentEnvelope();
	}
	
	public IEnumerable<PuzzleFragmentData> Gacha()

	{
		var collectID = DataManager.Save.Puzzle.Active;
		var result = new List<PuzzleFragmentData>();
		
		var noRepeats = true;
		for (var i = 0; i < Value; i++)
		{
			result.AddRange(new PuzzleFragmentData().Gacha(collectID,Type, noRepeats));
			noRepeats = false;
		}
		result = result.Merge();
		
		return result.Merge();
	}
	
	public ILootData Clone()
	{
		var clone = new PuzzleEnvelopeData
		{
			Type = Type,
			Value = Value,
		};

		return clone;
	}

	public bool Same(ILootData data)
	{
		var target = (PuzzleEnvelopeData) data;
		return target.Type.Equals(Type);
	}

	public void Add(ILootData data)
	{
		Value += ((PuzzleEnvelopeData)data).Value;
	}

	public void Multiple(int value)
	{
		Value *= value;
	}

	public double GetValue()
	{
		return Value;
	}
}
