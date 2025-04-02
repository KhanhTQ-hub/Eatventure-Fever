using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackListDevice", menuName = "BlackListDevice")]
public class BlackListDevice : ScriptableObject
{
    public List<string> data = new List<string>();

    [Button]
    public void CheckData()
    {
        var result = new List<string>();
        foreach (var item in data)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var low = item.ToLower();
                if (!result.Contains(low))
                    result.Add(low);
            }
        }

        data = result;
    }

    [Button]
    public void UpdateData(string content)
    {
        if(string.IsNullOrEmpty(content)) return;
        string[] strs = content.Split('|');
        foreach (var item in strs)
        {
            string[] str = item.Split('_');
            if (str.Length > 1)
            {
                if (str[0].CompareTo("Add") == 0)
                {
                    var id = str[1].ToLower();
                    if (!data.Contains(id))
                        data.Add(id);
                }

                if (str[0].CompareTo("Remove") == 0)
                {
                    var id = str[1].ToLower();
                    if (data.Contains(id))
                        data.Remove(id);
                }
            }
        }
    }

    public bool InBlacklist(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId)) return false;

        return data.Contains(deviceId);
    }
}
