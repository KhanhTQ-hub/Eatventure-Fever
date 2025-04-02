using System;
using System.Globalization;
using NPS.Loot;

[Serializable]
public class CurrencyData : ILootData
{
    public CurrencyType Type = CurrencyType.Coin;
    public double Value = 0;

    public CurrencyData()
    {
    }

    public CurrencyData(string content)
    {
        var str = content.Split(';');
        Enum.TryParse(str[1], out CurrencyType currencyType);
        Type = currencyType;
        Value = double.Parse(str[2], CultureInfo.InvariantCulture.NumberFormat);
    }

    public ILootData Clone()
    {
        var clone = new CurrencyData
        {
            Type = Type,
            Value = Value
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return Type == ((CurrencyData)data).Type;
    }

    public void Add(ILootData data)
    {
        Value += ((CurrencyData)data).Value;
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