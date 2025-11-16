using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DetachedReductorBehavior : MonoBehaviour
{
    private bool nearBallon = false;
    private Ballon tempBallon;
    public Rigidbody rb;
    public AudioSource hitSound;

    public void AttachToBallon()
    {
        if (nearBallon)
        {
            if (!tempBallon.withReductor)
            {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1f)
        {
            hitSound.Play();
        }
    }
}
