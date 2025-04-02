using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToolCheat : UIView, IPopup
{
#if UNITY_EDITOR || DEVELOPMENT || STAGING
    [Header("Feature config")]
    [SerializeField] TMP_InputField featureInput;
    [SerializeField] private TMP_Dropdown _dropdown;

    private List<PuzzleImageEntity> _imageEntity;
    public override void Show(object obj = null)
    {
        base.Show(obj);

        _dropdown.options.Clear();
        foreach (var collect in DataManager.Base.Puzzle.Collect.Values)
        {
            foreach (var image in collect.Image)
            {
                int count = 0;
                foreach (var fragment in image.Fragment)
                {
                    _dropdown.options.Add(new TMP_Dropdown.OptionData { text = $"{collect.Id}-{image.Index}-{count++}" });
                }
            }
        }

        _dropdown.value = 0;
        _dropdown.RefreshShownValue();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void ChangeFeatureRemote()
    {
        var featureConfig = featureInput.text;

        DataManager.Save.Save();
        TutorialManager.S?.InitData();
        FeatureController.S.TestFeatureConfig(featureConfig);

        var mapSave = DataManager.Save.Map;
        GameManager.S.KillAll();

        GameManager.S.Mode = GameMode.Normal;
        MonoScene.S.Load($"Zone{mapSave.Zone}_{mapSave.Level}");
    }

    public void AddFragment()
    {
        var content = _dropdown.options[_dropdown.value].text;
        var split = content.Split('-');
        var fragment = new PuzzleFragmentData
        {
            Collect = int.Parse(split[0]),
            Image = int.Parse(split[1]),
            Id = int.Parse(split[2]),
            Value = 1,
        };

        DataManager.Save.Puzzle.Collect[fragment.Collect].IncreaseFragment(fragment);
        DataManager.Save.Puzzle.Save();
    }
    
#endif
}
