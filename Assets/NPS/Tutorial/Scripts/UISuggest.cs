using TMPro;
using UnityEngine;

public class UISuggest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtSuggest;
    
    public void Set(string message)
    {
        txtSuggest.text = message;
    }
}
