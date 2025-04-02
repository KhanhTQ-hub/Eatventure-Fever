using NPS.Loot;
using System;
using System.Globalization;

[System.Serializable]
public class ChestData: ILootData
{
    public ChestType Type = ChestType.Wooden;
    public double Value = 0;

    public ChestData()
    {

    }

    public ChestData(string content)
    {
        var str = content.Split(';');
        Enum.TryParse(str[1], out ChestType type);
        Type = type;
        Value = double.Parse(str[2], CultureInfo.InvariantCulture.NumberFormat);
    }

    public ILootData Clone()
    {
        var clone = new ChestData
        {
            Type = Type,
            Value = Value
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return Type == ((ChestData)data).Type;
    }

    public void Add(ILootData data)
    {
        Value += ((ChestData)data).Value;
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
