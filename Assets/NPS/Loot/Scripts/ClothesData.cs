using NPS.Loot;
using System;
using System.Globalization;

[Serializable]
public class ClothesData : ILootData
{
    public int Id;
    public double Value = 0;
    public ClothesType Rarity;

    public ClothesData()
    {
        
    }

    public ClothesData(string content)
    {
        var str = content.Split(';');

        var isNumeric = int.TryParse(str[1], out int n);
        Id = isNumeric ? n : -1;
        if (!isNumeric)
        {
            Enum.TryParse(str[1], out ClothesType rarity);
            Rarity = rarity;
        }

        Value = double.Parse(str[2], CultureInfo.InvariantCulture.NumberFormat);
    }

    public ILootData Clone()
    {
        var clone = new ClothesData
        {
            Id = Id,
            Value = Value,
            Rarity = Rarity
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return (Id == ((ClothesData)data).Id && Id != -1) ||
               (Id == -1 && ((ClothesData)data).Id == -1 && Rarity == (data as ClothesData).Rarity);
    }

    public void Add(ILootData data)
    {
        Value += ((ClothesData)data).Value;
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