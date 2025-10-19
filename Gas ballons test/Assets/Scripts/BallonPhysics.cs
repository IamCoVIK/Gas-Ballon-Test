using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallonPhysics : MonoBehaviour
{
    public Rigidbody rb;
    public AudioSource hitSound;

    public UnityEvent hardHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1f)
        {
            hitSound.Play();
            if (rb.velocity.magnitude > 3f)
            {
                hardHit.Invoke();
            }
        }
    }
}
