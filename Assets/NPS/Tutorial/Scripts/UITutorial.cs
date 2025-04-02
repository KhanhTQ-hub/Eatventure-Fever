using com.unimob.mec;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace NPS.Tutorial
{
    public class UITutorial : UIView
    {
        #region Properties

        public Hand Hand => hand;

        [SerializeField] private GameObject lockBlack;
        [SerializeField] private GameObject lockTransparent;

        [SerializeField] private Transform suggestPos;
        [SerializeField] private UISuggest suggestPrb;

        [SerializeField] private UIHand uiHand;
        [SerializeField] private ObjHand objHand;

        [SerializeField] private GameObject objSkip;
        [SerializeField] private GameObject objTap2Continue;

        [SerializeField] private UIBoxChat boxChatPrb;

        private Hand hand;
        private UIBoxChat boxChat;
        private CoroutineHandle handMove;
        private UISuggest suggest;

        #endregion

        #region Handler 1

        public void Skip()
        {
            HideHand();
            HideBoxChat();
        }

        public void Tap2Continue()
        {
            objTap2Continue.SetActive(false);
            HideHand();
            HideBoxChat();
        }

        #endregion

        #region Handler 2

        public void ShowLock(LockType type)
        {
            lockBlack.SetActive(type == LockType.Black);
            lockTransparent.SetActive(type == LockType.Transparent);

            MainGameScene.S.LockTutorial = type != LockType.None;
        }

        public void ShowHand(bool isUI, HandType handT, LockType lockT, bool isSkip, GameObject parent,
            Vector3 offset = new Vector3(), params GameObject[] objs)
        {
            CreateHand(isUI, handT, parent.transform, offset);
            ShowLock(lockT);
            ShowSkip(isSkip);
            if (objs.Length == 0) RayCast(parent);
            else
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    RayCast(objs[i]);
                }
            }

            if (!isUI) Lean.S.Enable(false);
        }

        public void ShowHand(bool isUI, HandType handT, LockType lockT, bool isSkip, GameObject parent, Transform start,
            Transform end, params GameObject[] objs)
        {
            if (hand) Destroy(hand.gameObject);

            CreateHand(isUI, handT, parent.transform);
            ShowLock(lockT);
            ShowSkip(isSkip);

            for (int i = 0; i < objs.Length; i++)
            {
                RayCast(objs[i]);
            }

            if (handT == HandType.Move)
            {
                hand.Move(end, start, true);
            }

            if (!isUI) Lean.S.Enable(false);
        }

        public void ShowHand(bool isUI, HandType handT, LockType lockT, bool isSkip, GameObject parent, Vector2 start,
            Vector2 end, params GameObject[] objs)
        {
            if (hand) Destroy(hand.gameObject);

            CreateHand(isUI, handT, parent.transform);
            ShowLock(lockT);
            ShowSkip(isSkip);

            for (int i = 0; i < objs.Length; i++)
            {
                RayCast(objs[i]);
            }

            if (handT == HandType.Move)
            {
                handMove = Timing.RunCoroutine(hand._Move(end, start, true));
            }

            if (!isUI) Lean.S.Enable(false);
        }

        public void ShowLeftHand(bool isUI, HandType handT, LockType lockT, bool isSkip, GameObject parent,
            Vector3 offset = new Vector3(), params GameObject[] objs)
        {
            CreateLeftHand(isUI, handT, parent.transform, offset);
            ShowLock(lockT);
            ShowSkip(isSkip);
            if (objs.Length == 0) RayCast(parent);
            else
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    RayCast(objs[i]);
                }
            }

            if (!isUI) Lean.S.Enable(false);
        }

        private void ShowTap2Continue()
        {
            objTap2Continue.SetActive(true);
        }

        public void ShowBoxChat(string message, Transform parent)
        {
            HideBoxChat();

            boxChat = Instantiate(boxChatPrb, parent);

            boxChat.Set(message);
        }

        public void ShowText(string content, Transform parent = null)
        {
            HideText();

            Transform pos = null;
            pos = parent == null ? this.suggestPos : parent;
            suggest = Instantiate(suggestPrb, pos);

            suggest.Set(content);
        }

        private void ShowSkip(bool isShow)
        {
            objSkip.SetActive(isShow);
        }

        public void HideText()
        {
            if (suggest) Destroy(suggest.gameObject);
        }

        private void HideBoxChat()
        {
            if (boxChat) Destroy(boxChat.gameObject);
        }

        public void HideHand()
        {
            if (handMove.IsValid) Timing.KillCoroutines(handMove);

            if (hand) Destroy(hand.gameObject);

            Clear();

            Lean.S?.Enable();
        }

        #endregion

        #region Handler 3

        List<GraphicRaycaster> lstRc = new List<GraphicRaycaster>();
        List<Tuple<Canvas, bool, int>> lstCv = new List<Tuple<Canvas, bool, int>>();
        List<Tuple<Canvas, bool, int>> lstOldCv = new List<Tuple<Canvas, bool, int>>();
        List<GameObjectLayer> lstOldLayer = new List<GameObjectLayer>();
        List<Tuple<SortingGroup, int>> lstSg = new List<Tuple<SortingGroup, int>>();
        List<Tuple<SortingGroup, int>> lstOldSg = new List<Tuple<SortingGroup, int>>();

        public void RayCast(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("[Tutorial]: Raycast null");
                return;
            }

            obj.SetActive(true);

            int layer = obj.layer;
            if (layer == 5)
            {
                var cv = obj.GetComponent<Canvas>();
                if (cv == null)
                {
                    cv = obj.AddComponent<Canvas>();
                    lstCv.Add(new Tuple<Canvas, bool, int>(cv, cv.overrideSorting, cv.sortingOrder));
                }
                else
                {
                    lstOldCv.Add(new Tuple<Canvas, bool, int>(cv, cv.overrideSorting, cv.sortingOrder));
                }

                cv.overrideSorting = true;
                cv.sortingOrder = 303;

                var rc = obj.GetComponent<GraphicRaycaster>();
                if (rc == null)
                {
                    rc = obj.AddComponent<GraphicRaycaster>();
                    lstRc.Add(rc);
                }
            }
            else
            {
                ChangeLayer(obj, true);
            }
        }

        public void ChangeLayer(GameObject obj, bool isSave = false)
        {
            IChangeLayer(obj, LayerMask.NameToLayer("UI"), isSave);

            var sg = obj.GetComponent<SortingGroup>();
            if (sg == null)
            {
                sg = obj.AddComponent<SortingGroup>();
                lstSg.Add(new Tuple<SortingGroup, int>(sg, sg.sortingOrder));
            }
            else
            {
                lstOldSg.Add(new Tuple<SortingGroup, int>(sg, sg.sortingOrder));
            }

            sg.sortingOrder = 302;
        }

        public void AddLayer(GameObject obj)
        {
            var sg = obj.GetComponent<SortingGroup>();
            if (sg == null)
            {
                sg = obj.AddComponent<SortingGroup>();
                sg.sortingOrder = 303;
            }
            else
            {
                sg.sortingOrder += 1;
            }
        }

        private void IChangeLayer(GameObject obj, int layer, bool isSave = false)
        {
            if (isSave)
            {
                lstOldLayer.Add(new GameObjectLayer()
                {
                    obj = obj,
                    layer = obj.layer
                });
            }

            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                IChangeLayer(child.gameObject, layer, isSave);
            }
        }

        private void Clear()
        {
            foreach (var item in lstRc.Where(item => item != null))
            {
                Destroy(item);
            }

            foreach (var item in lstCv.Where(item => item != null && item.Item1 != null))
            {
                item.Item1.overrideSorting = item.Item2;
                item.Item1.sortingOrder = item.Item3;

                Destroy(item.Item1);
            }

            foreach (var item in lstOldCv.Where(item => item != null && item.Item1 != null))
            {
                item.Item1.overrideSorting = item.Item2;
                item.Item1.sortingOrder = item.Item3;

                if (item.Item1.overrideSorting != item.Item2)
                {
                    Debug.LogWarning("[Tutorial]: Don't change override sorting");
                }
            }

            foreach (var item in lstOldLayer.Where(item => item != null && item.obj != null))
            {
                item.obj.layer = item.layer;
            }

            foreach (var item in lstSg.Where(item => item != null && item.Item1 != null))
            {
                item.Item1.sortingOrder = item.Item2;

                Destroy(item.Item1);
            }

            foreach (var item in lstOldSg.Where(item => item != null && item.Item1 != null))
            {
                item.Item1.sortingOrder = item.Item2;
            }

            ShowLock(LockType.None);
            ShowSkip(false);

            lstRc.Clear();
            lstCv.Clear();
            lstOldCv.Clear();
        }

        private class GameObjectLayer
        {
            public GameObject obj;
            public int layer;
        }

        private void CreateHand(bool isUI, HandType type, Transform parent, Vector3 offset = new Vector3())
        {
            HideHand();

            hand = Instantiate(isUI ? (Hand) uiHand : objHand, parent);
            hand.gameObject.transform.localPosition += offset;

            hand.Set(type);
        }

        private void CreateLeftHand(bool isUI, HandType type, Transform parent, Vector3 offset = new Vector3())
        {
            HideHand();

            hand = Instantiate(isUI ? (Hand) uiHand : objHand, parent);
            hand.transform.localPosition += offset;
            hand.transform.localScale = new Vector3(-1, 1, 1);

            hand.Set(type);
        }

        #endregion
    }
}