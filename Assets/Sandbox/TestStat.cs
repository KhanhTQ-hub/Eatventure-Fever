using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestStat : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void Log()
    {
        string[] lines = inputField.text.Split('\n');

        string result = "";
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            string[] str = line.Split('|');
            for (int j = 0; j < str.Length; j++)
            {
                float number = float.Parse(str[j], CultureInfo.InvariantCulture.NumberFormat) - 1;

                result += number;
                if (j != str.Length - 1) result += "|";
            }

            result += "\n";
        }

        Debug.Log(result);
    }
}
