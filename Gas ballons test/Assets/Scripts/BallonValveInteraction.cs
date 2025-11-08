using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallonValveInteraction : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    public Animator valveAnimator;
    public AudioSource valveSpinSound;
    public AudioSource valveLeakSound;

    public float explosionTime;
    private float explosionTimer;
    private float ExplosionTimer
    {
        get 
        {
            return explosionTimer;
        }
        set
        {
            explosionTimer = value;
            if (explosionTimer <= 0)
            {
                Explosion.Invoke();
            }
        }
    }


    private bool isOpen = false;
    private Interactable interactable;
    private Ballon ballon;

    public UnityEvent Explosion;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        ExplosionTimer = explosionTime;
        ballon = GetComponentInParent<Ballon>();
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (grabAction.GetStateDown(hand.handType))
        {
            ToggleValve();
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            ExplosionTimer -= Time.deltaTime;
            /*if (explosionTimer <= 0 )
            {
                Explosion.Invoke();
            }*/
        }
    }

    void ToggleValve()
    {
        if (isOpen)
        {
            AnimateValve("Valve_Close");
            valveLeakSound.Stop();
            ExplosionTimer = explosionTime;
        }
        else
        {
            AnimateValve("Valve_Open");
            if (ballon.attachedReductor != null)
            {
                valveLeakSound.Play();
            }
        }

        isOpen = !isOpen;
        valveSpinSound.Play();
    }

    void AnimateValve(string anim)
    {
        valveAnimator.SetTrigger(anim);
    }
}
