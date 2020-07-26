using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        EventCenter.Instance.AddEventListener("Collection", Collect);
    }

    void Collect(object info)
    {
        Debug.Log(info);
    }

}
