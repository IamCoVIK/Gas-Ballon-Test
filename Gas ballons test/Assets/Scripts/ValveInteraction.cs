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

    void Start()
    {
        interactable = GetComponent<Interactable>();
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
        //if (reductor._attachedBallon != null && grabAction != null && grabAction.GetStateDown(hand.handType))
        if (reductor._attachedBallon != null && grabAction != null)
        {
            Debug.Log("Toggling");
            reductor.ToggleValve();
        }

    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint broke!");
        // Можно добавить обработку, если FixedJoint сломался (например, воспроизвести звук)
    }
}