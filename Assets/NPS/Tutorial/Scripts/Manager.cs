using System.Collections.Generic;
using System;
using NPS.Pattern.Observer;
using com.unimob.pattern.singleton;

namespace NPS.Tutorial
{
    public class Manager : MonoSingleton<TutorialManager>
    {
        private static UITutorial ui;
        protected List<int> QueueTut = new List<int>();
        public static UITutorial UI
        {
            get
            {
                if (!ui)
                    ui = MainGameScene.S.View<UITutorial>();
                return ui;
            }
        }

        protected TutorialSave save;

        protected virtual void Awake()
        {
            InitData();
        }

        public virtual void InitData()
        {
            save = DataManager.Save?.Tutorial;
        }

        public void Clear()
        {
            inits.Clear();
            nexts.Clear();
            completes.Clear();
            active.Clear();
        }

        #region Start

        private Dictionary<int, List<Action>> inits = new Dictionary<int, List<Action>>();
        private List<int> active = new List<int>();

        public void RegisterInit(int tut, Action action)
        {
            if (save.Complete.Contains(tut)) return;

            if (!inits.ContainsKey(tut))
                inits.Add(tut, new List<Action>());
            if (!inits[tut].Contains(action))
                inits[tut].Add(action);

            //Debug.Log("Tutorial: Register Init: " + tut);
        }

        public void RemoveActive(int value)
        {
            if (active.Contains(value))
                active.Remove(value);
        }

        public bool? Init(int tut)
        {
            var rs = ConditionInit(tut);
            if (rs)
            {
                //Debug.Log("Tutorial: Init: " + tut);

                active.Add(tut);

                save.CurTut = tut;
                save.CurStep = 1;

                Observer.S?.PostEvent(EventID.StartTutorial, tut);
                
                if (inits.ContainsKey(tut))
                {
                    foreach (var action in inits[tut])
                    {
                        action?.Invoke();
                    }
                    inits.Remove(tut);
                }

                save.Save();
            }

            return rs;
        }

        private bool ConditionInit(int tut)
        {
            if (active.Contains(tut)) return false;
            if (save.Complete.Contains(tut)) return false;
            if (save.CurTut != 0 && iConditionInit(tut))
            {
                // Tru 2 tut dac biet Puzzle va Rate ( ko uu tien )
                if (save.CurTut != 36 && save.CurTut != 18)
                {
                    if (!QueueTut.Contains(tut))
                    {
                        QueueTut.Add(tut);
                    }
                    return false;
                }

                RemoveActive(save.CurTut);
                return iConditionInit(tut);
            }
            return iConditionInit(tut);
        }

        protected virtual bool iConditionInit(int tut) => false;

        #endregion

        #region Next

        private Dictionary<int, List<Action>> nexts = new Dictionary<int, List<Action>>();

        public void RegisterNext(int tut, int step, Action action)
        {
            if (save.Complete.Contains(tut)) return;

            var key = tut * 100 + step;
            if (!nexts.ContainsKey(key))
                nexts.Add(key, new List<Action>());
            if (!nexts[key].Contains(action))
                nexts[key].Add(action);

            //Debug.Log("Tutorial: Register Next: " + tut + " / " + step);
        }

        public void Next(int tut, params int[] steps)
        {
            foreach (var step in steps)
            {
                Check(tut, step, () =>
                {
                    //Debug.Log("Tutorial: Next: " + tut + " / " + step);

                    var rs = Handler(tut, step);

                    var key = tut * 100 + step;
                    if (nexts.ContainsKey(key))
                    {
                        foreach (var action in nexts[key])
                        {
                            action?.Invoke();
                        }
                        nexts.Remove(key);
                    }

                    if (rs != null && !rs.Value) save.CurStep++;
                });
            }
        }

        public virtual bool? Handler(int tut, int step) => false;

        public bool? Check(int tut, int step, Action action = null)
        {
            if (save.Complete.Contains(tut)) return false;
            if (save.CurTut != tut || save.CurStep != step) return false;

            action?.Invoke();

            return true;
        }

        #endregion

        #region Complete

        private Dictionary<int, List<Action>> completes = new Dictionary<int, List<Action>>();

        public void RegisterComplete(int tut, Action action)
        {
            if (save.Complete.Contains(tut)) return;

            if (!completes.ContainsKey(tut))
                completes.Add(tut, new List<Action>());

            if (!completes[tut].Contains(action))
                completes[tut].Add(action);

            //Debug.Log("Tutorial: Register Complete: " + tut);
        }

        protected void Complete(int tut)
        {
            if (save.Complete.Contains(tut)) return;

            if (save.CurTut == tut)
            {
                //Debug.Log("Tutorial: Complete: " + tut);

                save.Complete.Add(tut);

                save.CurTut = 0;
                save.CurStep = 0;

                save.Save();
                Observer.S?.PostEvent(EventID.CompleteTutorial, tut);

                if (completes.ContainsKey(tut))
                {
                    foreach (var action in completes[tut])
                    {
                        action?.Invoke();
                    }
                    completes.Remove(tut);
                }    
                
                if (QueueTut.Count > 0)
                {
                    var value = QueueTut[0];
                    QueueTut.RemoveAt(0);
                    Init(value);
                }
            }
        }

        #endregion
    }
}