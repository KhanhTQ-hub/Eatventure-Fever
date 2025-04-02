using System.Collections.Generic;
using com.unimob.mec;
using UnityEngine;
using com.unimob.timer;
using kt.job.geometry;

namespace NPS.Tutorial
{
    public abstract class Hand : MonoBehaviour
    {
        [SerializeField] protected GameObject hand;
        [SerializeField] private float speed = 2;
        [SerializeField] private GameObject fx;

        private TickData tick = new TickData();

        public abstract void Set(HandType type);

        private void OnDisable()
        {
            tick.RemoveTick();
        }

        public void Move(Transform end, Transform start, bool isLoop = false)
        {
            fx.SetActive(true);

            tick.Action = () => Tick(end, start, isLoop);
            tick.RegisterTick();
        }

        private void Tick(Transform end, Transform start, bool isLoop)
        {
            var step = speed * Time.fixedDeltaTime;
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, end.position, step);

            if (transform.position.SqrMagnitudeTo(end.position) < 0.001f)
            {
                if (isLoop)
                {
                    fx.SetActive(false);
                    this.transform.position = start.position;
                    fx.SetActive(true);
                }
                else
                {
                    tick.RemoveTick();
                }
            }
        }
        
        public IEnumerator<float> _Move(Vector2 posEnd, Vector2 posStart, bool isLoop = false)
        {
            this.transform.position = posStart;
            fx.SetActive(true);

            while (true)
            {
                var step = speed * Time.fixedDeltaTime;
                this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, posEnd, step);
                if (Vector3.Distance(this.transform.position, posEnd) < 0.001f)
                {
                    if (isLoop)
                    {
                        fx.SetActive(false);
                        this.transform.position = posStart;
                        fx.SetActive(true);
                    }
                    else break;
                }

                yield return Timing.WaitForOneFrame;
            }
        }
    }
}