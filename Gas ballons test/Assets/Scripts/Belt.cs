using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] private float Height;
    [SerializeField] private Transform Head;

    void Update()
    {
        Vector3 currentPos = new Vector3(Head.position.x, Head.position.y - Height, Head.position.z);

        transform.position = currentPos;
        transform.rotation = Quaternion.Euler(0f, Head.rotation.eulerAngles.y, 0f);
    }
}
