using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NPS.Tutorial
{
    public class Init : MonoBehaviour
    {
        [SerializeField] private List<StartData> data;

        private void Awake()
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    Manager.S?.RegisterInit(item.Tut, () =>
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
    internal class StartData
    {
        public int Tut;
        public UnityEvent Event;
    }
}