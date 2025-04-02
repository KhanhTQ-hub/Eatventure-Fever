using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NPS.Tutorial
{
    public class Complete : MonoBehaviour
    {
        [SerializeField] private List<InitData> init;
        [SerializeField] private List<CompleteData> complete;

        private void Awake()
        {
            var save = DataManager.Save.Tutorial;
            foreach (var item in init)
            {
                if (save.Complete.Contains(item.Tut)) item.Complete?.Invoke();
                else item.UnComplete?.Invoke();
            }

            if (complete.Count > 0)
            {
                foreach (var item in complete)
                {
                    Manager.S?.RegisterComplete(item.Tut, () =>
                    {
                        item.Event?.Invoke();
                    });
                }
            }
        }

        private void Start()
        {

        }
    }

    [System.Serializable]
    internal class CompleteData
    {
        public int Tut;
        public UnityEvent Event;
    }

    [System.Serializable]
    internal class InitData
    {
        public int Tut;
        public UnityEvent Complete;
        public UnityEvent UnComplete;
    }
}