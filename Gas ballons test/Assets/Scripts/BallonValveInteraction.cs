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

    private bool isOpen = false;
    private Interactable interactable;

    public UnityEvent Explosion;

    void Start()
    {
        interactable = GetComponent<Interactable>();
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
            explosionTime -= Time.deltaTime;
            if (explosionTime <= 0 )
            {
                Explosion.Invoke();
            }
        }
    }

    void ToggleValve()
    {
        if (isOpen)
        {
            AnimateValve("Valve_Close");
            valveLeakSound.Stop();
        }
        else
        {
            AnimateValve("Valve_Open");
            valveLeakSound.Play();
        }

        isOpen = !isOpen;
        valveSpinSound.Play();
    }

    void AnimateValve(string anim)
    {
        valveAnimator.SetTrigger(anim);
    }
}
