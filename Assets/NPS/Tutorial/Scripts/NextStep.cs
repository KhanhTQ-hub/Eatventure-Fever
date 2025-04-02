using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NPS.Tutorial
{
    public class NextStep : MonoBehaviour
    {
        [SerializeField] private List<NextStepData> data;

        private void Awake()
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    Manager.S?.RegisterNext(item.Tut, item.Step, () =>
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
    public class NextStepData
    {
        public int Tut;
        public int Step;
        public UnityEvent Event;
    }
}