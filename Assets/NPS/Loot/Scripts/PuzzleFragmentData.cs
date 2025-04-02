using System;
using System.Collections.Generic;
using System.Globalization;
using com.unimob.serialize.core;
using NPS.Loot;
using UnityEngine;

[Serializable]
public class PuzzleFragmentData : ILootData
{
    public PuzzleImageSave ImageSave => CollectSave.Image[Image];

    public PuzzleCollectSave CollectSave => DataManager.Save.Puzzle.Collect[Collect];

    public int Index;
    public int Collect = -1; // Album nao
    public double Value;
    public int Image; // Bo Image nao
    public int Id; //id cua manh
    
    public PuzzleFragmentData()
    {
    }

    public PuzzleFragmentData(string content)
    {
        var str = content.Split(';');

        var s = 1;
        if (str.Length > 3)
        {
            Collect = int.Parse(str[0]);
            s++;
        }
        
        Image = int.Parse(str[s]);
        Id = int.Parse(str[s + 1]);
        Value = double.Parse(str[s + 2], CultureInfo.InvariantCulture);
    }

    public List<PuzzleFragmentData> Gacha(int collect, int star, bool isNoRepeats)
    {
        var result = new List<PuzzleFragmentData>();
        var entity = DataManager.Base.Puzzle.Collect[collect];

        if (star == -1)
        {
            var puzzleSave = DataManager.Save.Puzzle;
            var gameConfig = DataManager.Live.GameConfig;
            if (puzzleSave.CountFiveStar < gameConfig.CountFiveStar)
            {
                result.Add(puzzleSave.CountFourStar >= gameConfig.CountFourStar && isNoRepeats ? entity.RandomFragmentStar(4) : entity.RandomFragment());
            }
            else
            {
                result.Add(isNoRepeats ? entity.RandomFragmentStar(5) : entity.RandomFragment());
            }
        }
        else
        {
            result.Add(entity.RandomFragmentStar(star));
        }

        return result.Merge();
    }
    
    public ILootData Clone()
    {
        var clone = new PuzzleFragmentData
        {
            Index = Index,
            Collect = Collect,
            Image = Image,
            Id = Id,
            Value = Value,
        };

        return clone;
    }

    public bool Same(ILootData data)
    {
        var target = (PuzzleFragmentData) data;
        return target.Collect.Equals(Collect) && target.Image.Equals(Image) && target.Id.Equals(Id);
    }

    public void Add(ILootData data)
    {
        Value += ((PuzzleFragmentData)data).Value;
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