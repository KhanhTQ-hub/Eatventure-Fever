using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisible : MonoBehaviour
{
    [Button]
    public void Test()
    {
        Debug.Log("Visible: " + this.transform.position.IsObjectVisible());
    }
}
