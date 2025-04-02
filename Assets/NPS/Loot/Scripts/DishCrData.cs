using NPS.Loot;
using System;
using System.Globalization;

[Serializable]
public class DishCrData : ILootData
{
    public int Id;
    public double Value = 0;
    public Rarity Rarity;

    public DishCrData()
    {

    }

    public DishCrData(string content)
    {
        var str = content.Split(';');

        var isNumeric = int.TryParse(str[1], out var n);
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
        var clone = new DishCrData
        {
            Id = Id,
            Value = Value,
            Rarity = Rarity
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return (Id == ((DishCrData)data).Id && Id != -1) || (Id == -1 && ((DishCrData)data).Id == -1 && Rarity == (data as DishCrData).Rarity);
    }

    public void Add(ILootData data)
    {
        Value += ((DishCrData)data).Value;
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
