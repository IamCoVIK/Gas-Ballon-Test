using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandPockets : MonoBehaviour
{
    public UnityEvent OnResetHandPocketItems;
    public UnityEvent OnDeactivatingHandPocketItems;
    public UnityEvent OnActivatingHandPocketItems;

    public bool activated;

    public void ResetHandPocketItems()
    {
        OnResetHandPocketItems.Invoke();
    }

    public void DeactivateHandPockets()
    {
        OnDeactivatingHandPocketItems.Invoke();
        activated = false;
        //gameObject.SetActive(false);
    }

    public void ActivateHandPockets()
    {
        OnActivatingHandPocketItems.Invoke();
        activated = true;
        //gameObject.SetActive(true);
    }

    private void Start()
    {
        DeactivateHandPockets();
    }
}
