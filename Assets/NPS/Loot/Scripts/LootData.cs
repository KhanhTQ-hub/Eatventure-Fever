using System;

namespace NPS.Loot
{
    [Serializable]
    public class LootData
    {
        public LootType Type;
        public ILootData Data;

        public LootData()
        {
        }

        public LootData(string content)
        {
            var str = content.Split(';');
            Enum.TryParse(str[0], out LootType lootType);
            Type = lootType;

            Data = lootType switch
            {
                LootType.Currency => new CurrencyData(content),
                LootType.Chest => new ChestData(content),
                LootType.Equipment => new EquipmentData(content),
                LootType.Special => new SpecialRwData(content),
                LootType.Key => new KeyData(content),
                LootType.Dish => new DishCrData(content),
                LootType.Potion => new PotionData(content),
                LootType.Clothes => new ClothesData(content),
                LootType.PuzzleFragment => new PuzzleFragmentData(content),
                LootType.PuzzleEnvelope => new PuzzleEnvelopeData(content),
                _ => null
            };
        }

        public LootData Clone()
        {
            var clone = new LootData
            {
                Type = Type,
                Data = Data.Clone()
            };
            return clone;
        }

        public bool Same(LootData data)
        {
            return Type == data.Type && data.Data.Same(Data);
        }
    }
}