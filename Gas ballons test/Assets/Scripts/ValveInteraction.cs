using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ValveInteraction : MonoBehaviour
{
    private Interactable interactable;
    private Hand currentHand;
    private FixedJoint fixedJoint;
    public Reductor reductor;

    public SteamVR_Action_Boolean grabAction = SteamVR_Actions.default_GrabPinch;

    private bool block;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        block = false;
    }

    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log("Hand hovering over valve.");
    }

    private void OnHandHoverEnd(Hand hand)
    {
        Debug.Log("Hand no longer hovering over valve.");
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (block)
        {
            if (grabAction.GetStateUp(hand.handType))
            {
                block = false;
            }
            return;
        }
        if (reductor._attachedBallon != null && grabAction != null && grabAction.GetStateDown(hand.handType))
        {
            block = true;
            if (reductor.IsValveOpen)
            {
                reductor.CloseValve();
            }
            else
            {
                reductor.OpenValve();
            }
        }

    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint broke!");
        // Можно добавить обработку, если FixedJoint сломался (например, воспроизвести звук)
    }
}