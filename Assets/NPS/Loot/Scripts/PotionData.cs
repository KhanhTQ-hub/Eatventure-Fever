using System;
using System.Globalization;
using NPS.Loot;

[Serializable]
public class PotionData : ILootData
{
    public DishCreativeType Type = DishCreativeType.Bitter;
    public double Value = 0;

    public PotionData()
    {
    }

    public PotionData(string content)
    {
        var str = content.Split(';');
        Enum.TryParse(str[1], out DishCreativeType PotionType);
        Type = PotionType;
        Value = double.Parse(str[2], CultureInfo.InvariantCulture.NumberFormat);
    }

    public ILootData Clone()
    {
        var clone = new PotionData
        {
            Type = Type,
            Value = Value
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return Type == ((PotionData)data).Type;
    }

    public void Add(ILootData data)
    {
        Value += ((PotionData)data).Value;
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