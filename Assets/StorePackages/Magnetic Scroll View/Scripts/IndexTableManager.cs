﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
//using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine.UI;

namespace MagneticScrollView
{
    /// <summary>
    /// Manages how the Index Table should behave.
    /// </summary>

#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [SelectionBase]
    [DisallowMultipleComponent]
    public class IndexTableManager : MonoBehaviour
    {
        #region FIELDS
        [Tooltip ("Reference to Index Prefab, the indexes will be automatically modified")]
        [SerializeField, HideInInspector] public GameObject indexPrefab;
        [Tooltip ("Reference to Indicator Prefab, the indicator will be automatically modified")]
        [SerializeField, HideInInspector] public GameObject indicatorPrefab;

        [Tooltip ("Indexes alignment ordering (Horizontal or Vertical).")]
        [SerializeField] private Alignment m_alignment = Alignment.Horizontal;
        [SerializeField] private MagneticScrollRect scrollRect;
        [SerializeField] private GameObject indicator;
        //[SerializeField] private List<GameObject> indexes;
        [SerializeField] private Vector2 indicatorSize = new Vector2 (0.5f, 0.5f);
        [SerializeField] private Vector2 indexSize;


        //[SerializeField] private Sprite knobSprite;
        //[SerializeField] private Sprite bgSprite;


        private int elementCount = 0;
        private int prefabSavingFrame;

        //IEnumerator SavePrefabCoroutine;
        #endregion

        //#region Constants

        //private const string bgSpritePath = "UI/Skin/Background.psd";
        //private const string knobPath = "UI/Skin/Knob.psd";

        //#endregion

        #region PROPERTIES
        /// <summary>
        /// Indexes alignment ordering (Horizontal or Vertical).
        /// </summary>
        public Alignment AlignmentEnum
        {
            get { return m_alignment; }
            set
            {
                m_alignment = value;
                ChangeLayoutGroup ();
            }
        }
        /// <summary>
        /// Indexes alignment ordering (Horizontal or Vertical).
        /// </summary>
        public int AlignmentInt
        {
            get { return (int)m_alignment; }
            set { AlignmentEnum = (Alignment)value; }
        }
        #endregion

        #region Methods        
        //private void OnAwake ()
        //{
        //    Debug.Log ("Awaken");
        //    RemovePrefabs ();
        //}

        private void LateUpdate ()
        {
            elementCount = scrollRect.Elements.Count(x => x.gameObject.activeSelf);

            if (transform.childCount != elementCount/* || transform.childCount != transform.childCount || transform.childCount != viewportChildCount*/)
            {
                //Debug.Log (Time.frameCount);
                //Debug.Log (gameObject, gameObject);
                SetupIndexes ();
            }

            //RemovePrefabs ();
            if (indicator == null)
                SetupIndicator ();

            RemoveNonIndicatorIndexChildren ();
        }

        private void OnEnable ()
        {
            //Debug.Log ("Enabled");

#if UNITY_EDITOR

            //EditorApplication.update += EditorUpdate;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCallBack;

            //knobSprite = AssetDatabase.GetBuiltinExtraResource<Sprite> (knobPath);
            //bgSprite = AssetDatabase.GetBuiltinExtraResource<Sprite> (bgSpritePath);
            //#else
            //knobSprite = Resources.GetBuiltinResource<Sprite> (knobPath);
            //bgSprite = Resources.GetBuiltinResource<Sprite> (bgSpritePath);

#endif
            scrollRect = GetComponentInParent<MagneticScrollRect> ();

            elementCount = scrollRect.Elements.Count(x => x.gameObject.activeSelf);

            SetupIndexes ();
            ResizeIndicator ();
            ChangeLayoutGroup ();
        }

#if UNITY_EDITOR
        private void OnDisable ()
        {
            //EditorApplication.update -= EditorUpdate;
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyItemCallBack;
        }

        private void HierarchyItemCallBack (int instance, Rect rect)
        {
            Event evt = Event.current;
            if (evt.type == EventType.ValidateCommand && evt.commandName == "UndoRedoPerformed")
            {
                //AssignIndexes ();
                SetupIndexes ();
            }
        }

        private void ReplaceIndicator ()
        {
            Undo.RecordObject (this, "Removing objects from list");

            if (indicator != null)
            {
                AdvancedDestroy (indicator, true);
            }

            if (transform.childCount > 0)
            {
                foreach (Transform child in transform.GetChild (0))
                {
                    bool isPrefabInstance;

#if UNITY_2018_3_OR_NEWER
                    isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance (child);
#else
                    isPrefabInstance = PrefabUtility.GetPrefabParent (child) != null;
#endif
                    if (isPrefabInstance)
                    {
                        indicator = child.gameObject;
                        break;
                    }
                }

                if (transform.GetChild (0) != null && indicator == null)
                {
                    indicator = NewUIObject ("Indicator", indicatorPrefab, transform.GetChild (0).transform, Color.black);
                    ResizeIndicator ();
                }
            }
        }
#endif

        /// <summary>
        /// Move Indicator to the target index position.
        /// </summary>
        /// <param name="index">The target index</param>
        public void MoveIndicator (int index, bool invertOrder)
        {
            if (indicator != null && transform.childCount > index && transform.GetChild (index) != null)
            {
                if (invertOrder)
                    index = (transform.childCount - 1) - index;
                //Debug.Log ("Moved");


#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    indicator.transform.SetParent (transform.GetChild (index).transform, false);
                }
                else
                {
                    bool isPrefabInstance;

#if UNITY_2018_3_OR_NEWER
                    isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance (indicator);
#else
                    isPrefabInstance = PrefabUtility.GetPrefabParent (indicator) != null;
#endif
                    if (isPrefabInstance)
                    {
                        Undo.SetTransformParent (indicator.transform, transform.GetChild (index).transform, "Setting indicator parent");
                    }
                }
#else
                indicator.transform.SetParent (transform.GetChild(index).transform, false);
#endif


                indicator.transform.localPosition = Vector2.zero;
            }
        }

        private void SetupIndicator ()
        {
            //Debug.Log (indicator);            

            if (transform.childCount > 0)
            {
#if UNITY_EDITOR

                foreach (Transform child in transform.GetChild (0))
                {
                    bool isPrefabInstance = false;
#if UNITY_2018_3_OR_NEWER
                    isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance (indicator);
#else
                    isPrefabInstance = PrefabUtility.GetPrefabParent (indicator) != null;
#endif
                    if (isPrefabInstance)
                    {
                        indicator = child.gameObject;
                        break;
                    }
                }
#endif

                if (indicator == null)
                {
                    indicator = NewUIObject ("Indicator", indicatorPrefab, transform.GetChild (0).transform, Color.black);
                }
            }

            if (indicator != null)
            {
                if (transform.childCount > 0)
                {
                    if (scrollRect != null && scrollRect.Elements != null && scrollRect.CurrentSelectedIndex < scrollRect.Elements.Length && scrollRect.CurrentSelectedIndex >= 0)
                    {
                        indicator.transform.SetParent (transform.GetChild (scrollRect.CurrentSelectedIndex));
                    }
                    else
                        indicator.transform.SetParent (transform.GetChild (0));

                    indicator.name = "Indicator";
                    indicator.layer = 5;
                    indicator.transform.localPosition = Vector3.zero;
                }
                else
                {
                    AdvancedDestroy (indicator);
                }
            }

            ResizeIndicator ();
        }

        private void ResizeIndicator ()
        {
            if (indicator != null)
            {
                RectTransform indicatorRT = indicator.GetComponent<RectTransform> ();

                if (indicatorRT != null)
                {

                    indicatorRT.sizeDelta = Vector2.zero;
                    indicatorRT.anchorMin = Vector2.zero;
                    indicatorRT.anchorMax = Vector2.one;

                    indicatorRT.sizeDelta = new Vector2 (indexSize.x * Mathf.Lerp (-1f, 1f, indicatorSize.x / 2f), indexSize.y * Mathf.Lerp (-1f, 1f, indicatorSize.y / 2f));
                    //Debug.Log (indicatorRT.sizeDelta);
                }
            }
        }

        private void RemoveNonIndicatorIndexChildren ()
        {
            //Removes non Indicator objects.
            for (int i = 0; i < transform.childCount; i++)
            {
                //Debug.Log (transform.GetChild(i));
                foreach (Transform child in transform.GetChild (i).transform)
                {
                    if (child.gameObject != indicator)
                    {
                        AdvancedDestroy (child.gameObject);
                        Debug.Log (child.gameObject, child.gameObject);
                    }
                }
            }

        }

        private void SetupIndexes ()
        {
            var list = new List<Transform> ();
            foreach (Transform child in transform)
                list.Add (child);

            for (int i = 0; i < list.Count && transform.childCount > elementCount; i++)
            {
                AdvancedDestroy (list [i].gameObject, false, true);
            }

            //if (transform.childCount < elementCount && PrefabStageUtility.GetPrefabStage (gameObject) == null && !PrefabUtility.IsPartOfPrefabAsset (gameObject))
            for (int i = transform.childCount; i < elementCount; i++)
                NewUIObject ("Index" + (i + 1).ToString (), indexPrefab, transform, Color.white);

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild (i).name = "Index " + (i + 1).ToString ();
            //StartCoroutine (CreateIndexes ());

            ResizeIndexes ();
            SetupIndicator ();
        }

#if UNITY_EDITOR
        public void ReplaceIndex ()
        {
            var list = new List<Transform> ();
            foreach (Transform child in transform)
                list.Add (child);

            for (int i = 0; i < list.Count; i++)
            {
                AdvancedDestroy (list [i].gameObject, false, true);
            }

            int childCount = transform.childCount;
            //if (transform.childCount < elementCount && PrefabStageUtility.GetPrefabStage (gameObject) == null && !PrefabUtility.IsPartOfPrefabAsset (gameObject))
            for (int i = 0; i < elementCount; i++)
                NewUIObject ("Index" + (i + 1).ToString (), indexPrefab, transform, Color.white);

            ResizeIndexes ();
            SetupIndicator ();
        }
#endif

        private GameObject NewUIObject (string name, GameObject prefab, Transform parent, /*Sprite sprite,*/ Color color)
        {

            GameObject GO;
            if (prefab != null)
            {
                GO = SafeOperations.Instantiate (prefab, parent);
                GO.name = name;
            }
            else
            {
                GO = SafeOperations.NewGameObject (name, parent, typeof (RectTransform), typeof (CanvasRenderer));
                Image img = GO.SafeAddComponent<Image> ();
                //img.sprite = sprite;
                img.color = color;
                GO.layer = 5;
            }

            return GO;
        }


        private void ResizeIndexes ()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild (i) == null)
                    return;

                RectTransform indexRT = transform.GetChild (i).GetComponent<RectTransform> ();
                RectTransform rt_Table = GetComponent<RectTransform> ();
                if (indexRT != null && rt_Table != null)
                {
                    float indexSizeFactor = 0.95f;
                    Vector2 tableSize = new Vector2 (rt_Table.rect.width, rt_Table.rect.height);
                    float sizeChanged =
                        Mathf.Clamp (Mathf.Max (tableSize.x, tableSize.y) / elementCount, 0f, Mathf.Min (tableSize.x, tableSize.y)) * indexSizeFactor;
                    indexSize = new Vector2 (sizeChanged, sizeChanged);

                    indexRT.sizeDelta = indexSize;
                }
            }
        }

        private void ChangeLayoutGroup ()
        {
            RectTransform tableRT = GetComponent<RectTransform> ();
            Vector2 oldRectSize = new Vector2 (tableRT.rect.width, tableRT.rect.height);

            float minSize = Mathf.Min (oldRectSize.x, oldRectSize.y);
            float maxSize = Mathf.Max (oldRectSize.x, oldRectSize.y);

            if (m_alignment == Alignment.Vertical)
            {
                if (GetComponent<VerticalLayoutGroup> () == null)
                {
                    HorizontalLayoutGroup hGroup = GetComponent<HorizontalLayoutGroup> ();
                    if (hGroup)
                        SafeOperations.Destroy (hGroup);

                    VerticalLayoutGroup layoutGroup = gameObject.SafeAddComponent<VerticalLayoutGroup> ();
                    tableRT.sizeDelta = new Vector2 (minSize, maxSize);
                    layoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    layoutGroup.childControlWidth = false;
                    layoutGroup.childControlHeight = false;
                }
            }
            else
            {
                if (GetComponent<HorizontalLayoutGroup> () == null)
                {
                    VerticalLayoutGroup vGroup = GetComponent<VerticalLayoutGroup> ();
                    if (vGroup)
                        SafeOperations.Destroy (vGroup);

                    HorizontalLayoutGroup layoutGroup = gameObject.SafeAddComponent<HorizontalLayoutGroup> ();
                    tableRT.sizeDelta = new Vector2 (maxSize, minSize);
                    layoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    layoutGroup.childControlWidth = false;
                    layoutGroup.childControlHeight = false;
                }
            }
        }

        private void OnRectTransformDimensionsChange ()
        {
            ResizeIndexes ();
            ResizeIndicator ();
        }

        private void AdvancedDestroy (GameObject obj, bool saveUndo = false, bool allowDestroyingAsset = false)
        {
#if UNITY_EDITOR
            bool isPrefabInstance = false;
#if UNITY_2018_3_OR_NEWER
            isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance (obj) && !PrefabUtility.IsOutermostPrefabInstanceRoot (obj);
#else
            isPrefabInstance = PrefabUtility.GetPrefabParent (indicator) == null && PrefabUtility.FindPrefabRoot (obj) != obj;
#endif
            if (isPrefabInstance)
                return;
#endif

            if (saveUndo)
                SafeOperations.Destroy (obj);
            else
            {
                if (Application.isPlaying)
                    Destroy (obj);
                else
                    DestroyImmediate (obj, allowDestroyingAsset);
            }

        }
        #endregion
    }
}