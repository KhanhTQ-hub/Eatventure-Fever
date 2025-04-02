using UnityEngine;

public class AutoTestItem : MonoBehaviour
{
#if DEVELOPMENT || UNITY_EDITOR || STAGING
    [SerializeField] private GameObject active;

    public virtual void Set()
    {

    }

    public virtual void OnClick(bool value)
    {
        active.SetActive(value);
    }
#endif
}
