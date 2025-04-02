using NPS.Loot;
using System;
using System.Globalization;

[Serializable]
public class EquipmentData : ILootData
{
    public int Id;
    public double Value = 0;
    public Rarity Rarity;

    public EquipmentData()
    {

    }

    public EquipmentData(string content)
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
        var clone = new EquipmentData
        {
            Id = Id,
            Value = Value,
            Rarity = Rarity
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        return (Id == ((EquipmentData)data).Id && Id != -1) || (Id == -1 && ((EquipmentData)data).Id == -1 && Rarity == (data as EquipmentData).Rarity);
    }

    public void Add(ILootData data)
    {
        Value += ((EquipmentData)data).Value;
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
