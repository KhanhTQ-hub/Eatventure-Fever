using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using NPS.Pattern.Observer;

public class UIStatTest : MonoBehaviour
{
#if DEVELOPMENT || UNITY_EDITOR || STAGING
    [SerializeField] private DictionaryStat stats;
    [SerializeField] private TextMeshProUGUI txtContent;

    private void OnEnable()
    {
        this.RegisterListener(EventID.ChangeStat, OnChangeStat);

        UpdateUI();
    }

    private void OnChangeStat(object obj)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        var str = "";
        foreach (var item in stats.Keys)
        {
            var stat = stats.Get(item);
            if (stat > 1 || (Constant.RateStat.Contains(item) && stat > 0))
            {
                str += $"{item} : {stat}\n";
            }
        }
        txtContent.text = str;
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ChangeStat, OnChangeStat);
    }

#endif
}
