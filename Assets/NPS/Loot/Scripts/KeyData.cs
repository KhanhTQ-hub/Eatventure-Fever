using System;
using System.Globalization;
using NPS.Loot;

[Serializable]
public class KeyData : ILootData
{
    public KeyType Type = KeyType.Silver;
    public double Value = 0;

    public KeyData()
    {
    }

    public KeyData(string content)
    {
        var str = content.Split(';');
        Enum.TryParse(str[1], out KeyType keyType);
        Type = keyType;
        Value = double.Parse(str[2], CultureInfo.InvariantCulture.NumberFormat);
    }

    public ILootData Clone()
    {
        var clone = new KeyData
        {
            Type = Type,
            Value = Value
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return Type == ((KeyData)data).Type;
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
}