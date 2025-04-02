using System;
using System.Globalization;
using NPS.Loot;

[Serializable]
public class PuzzleImageData : ILootData
{ 
    public PuzzleImageEntity Entity => DataManager.Base.Puzzle.Collect[Collect].Image[Id];
    
    public int Collect = -1; // Album nao
    public double Value;
    public int Id;
    
    public PuzzleImageData()
    {
    }

    public PuzzleImageData(string content)
    {
        var str = content.Split(';');

        Collect = int.Parse(str[0]);
        Id = int.Parse(str[1]);
        Value = double.Parse(str[2]);
    }

    public ILootData Clone()
    {
        var clone = new PuzzleImageData
        {
            Collect = Collect,
            Id = Id,
            Value = Value
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        var target = (PuzzleImageData) data;
        return target.Collect.Equals(Collect) && target.Id.Equals(Id);
    }

    public bool CheckSameData(PuzzleImageData data)
    {
        return Collect == data.Collect && Id == data.Id;
    }
    
    public void Add(ILootData data)
    {
        Value += ((KeyData)data).Value;
    }

    public void Multiple(int value)
    {
        Value *= value;
    }

    public double GetValue()
    {
        return Value;
    }

    public void NextData()
    {
        Id++;
    }
    
    public void PrevData()
    {
        Id--;
    }
}