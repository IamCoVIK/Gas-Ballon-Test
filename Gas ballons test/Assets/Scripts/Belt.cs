using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{

    private void Awake()
    {   
        Vector3 a = transform.localPosition;
        a.Set(a.x, -0.5f, a.z);
        transform.localPosition = a;
    }

    void Update()
    {
        transform.rotation.Normalize();
    }
}
