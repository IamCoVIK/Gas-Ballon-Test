using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonPhysics : MonoBehaviour
{
    public Rigidbody rb;
    public AudioSource hitSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1f)
        {
            hitSound.Play();
        }
    }
}
