using UnityEngine;
using TMPro;

namespace NPS.Tutorial
{
    public class UIBoxChat : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txtMessage = null;

        public void Set(string message)
        {
            txtMessage.text = message;
        }
    }
}