using UnityEngine;
#if DEVELOPMENT || UNITY_EDITOR || STAGING
using NPS.Pattern.Observer;
#endif

public class UICollectChangeEvent : MonoBehaviour
{
#if DEVELOPMENT || UNITY_EDITOR || STAGING

    [SerializeField] private UIItemChangeEvent itemPrb;
    [SerializeField] private Transform content;

    private RemoteConfigSave remote;
    
#endif
}
