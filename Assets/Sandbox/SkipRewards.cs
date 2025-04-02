using NPS.Loot;
using UnityEngine;

public class SkipRewards : MonoBehaviour
{
    public static void Skip()
    {
        var collectionSave = DataManager.Save.Collection;
        var special = DataManager.Save.Special;

        special.SkipRewawds();
        special.Save();
        
        collectionSave.SkipRewards();
        collectionSave.Save();
    }
}