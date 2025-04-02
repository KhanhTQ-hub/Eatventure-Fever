using NPS.Loot;
using System;
using System.Globalization;

[Serializable]
public class SpecialRwData : ILootData
{
    public int Id;
    public double Value = 0;
    public Rarity Rarity;

    public SpecialRwData()
    {
    }

    public SpecialRwData(string content)
    {
        var str = content.Split(';');

        var isNumeric = int.TryParse(str[1], out int n);
        Id = isNumeric ? n : -1;
        if (!isNumeric)
        {
            Enum.TryParse(str[1], out Rarity rarity);
            Rarity = rarity;
        }

        Value = double.Parse(str[2], CultureInfo.InvariantCulture.NumberFormat);
    }

    public ILootData Clone()
    {
        var clone = new SpecialRwData
        {
            Id = Id,
            Value = Value,
            Rarity = Rarity
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return (Id == ((SpecialRwData)data).Id && Id != -1) || (Id == -1 && ((SpecialRwData)data).Id == -1 && Rarity == (data as SpecialRwData).Rarity);
    }

    public void Add(ILootData data)
    {
        Value += ((SpecialRwData)data).Value;
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