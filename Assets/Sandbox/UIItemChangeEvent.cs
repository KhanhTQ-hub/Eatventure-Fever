using UnityEngine;
using UnityEngine.UI;
#if DEVELOPMENT || UNITY_EDITOR || STAGING
using EventType = NPS.Remote.EventType;
#endif

public class UIItemChangeEvent : MonoBehaviour
{
#if DEVELOPMENT || UNITY_EDITOR || STAGING

    [SerializeField] private Image imgIcon;

    private EventType type;

    public void Set(EventType type)
    {
        this.type = type;
        imgIcon.sprite = ResourceManager.S.LoadSprite("IconEvent", type.ToString(), false);
        imgIcon.SetNativeSize();
    }

#endif
}
