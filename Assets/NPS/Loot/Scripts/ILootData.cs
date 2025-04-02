namespace NPS.Loot
{
    public interface ILootData
    {
        ILootData Clone();
        bool Same(ILootData data);
        void Add(ILootData data);
        void Multiple(int value);
        double GetValue();
    }
}