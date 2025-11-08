using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DetachedReductorBehavior : MonoBehaviour
{
    private bool nearBallon = false;
    private Ballon tempBallon;

    public void AttachToBallon()
    {
        Debug.Log("1");
        if (nearBallon)
        {
            Debug.Log("2");
            if (!tempBallon.withReductor)
            {
                Debug.Log("3");
                tempBallon.AttachReductor();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BallonConnector"))
        {
            nearBallon = true;
            tempBallon = other.GetComponentInParent<Ballon>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BallonConnector"))
        {
            nearBallon = false;
            tempBallon = null;
        }
    }
}
