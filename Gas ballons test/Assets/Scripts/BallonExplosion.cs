using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonExplosion : MonoBehaviour
{
    public GameObject explosion;
    public GameObject ballon;

    public void Explode()
    {
        Instantiate(explosion, ballon.transform.position, ballon.transform.rotation);

        GameObject.FindWithTag("TestingSystem").GetComponent<TestingSystem>().FailedTest();

        Destroy(ballon);
    }
}
