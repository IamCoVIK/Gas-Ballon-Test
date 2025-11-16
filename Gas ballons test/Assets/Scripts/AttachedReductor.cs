using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class AttachedReductor : MonoBehaviour
{
    private Interactable interactable;
    public Ballon ballon;
    public GameObject detachedReductor;

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    public Animator animator;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (grabAction.GetStateDown(hand.handType))
        {
            //GameObject reductorTemp = Instantiate(detachedReductor);
            hand.AttachObject(Instantiate(detachedReductor), GrabTypes.Grip);
            ballon.DetachReductor();
        }
    }

    public void ArrowUp(float pressure)
    {
        animator.speed = 1f;
        animator.SetTrigger("BallonValveToggle");
        Invoke(nameof(StopAnimation), 0.2f * (pressure / 20.27f));
    }

    public void ArrowDown()
    {
        animator.speed = 1f;
        animator.SetTrigger("BallonValveToggle");
    }

    private void StopAnimation()
    {
        animator.speed = 0f;
    }
}
