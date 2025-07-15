using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ReductorInteraction : MonoBehaviour
{
    private Interactable interactable;
    private Hand currentHand;
    private FixedJoint fixedJoint; // ƒл€ креплени€ баллона к руке
    public Transform ValveHandle;
    public Reductor reductor;

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log("Hand hovering over reductor.");
    }

    private void OnHandHoverEnd(Hand hand)
    {
        Debug.Log("Hand no longer hovering over reductor.");
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (reductor.IsAttachedAndLocked) return;
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        // ≈сли редуктор еще не захвачен и начинаетс€ захват
        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            // ≈сли редуктор присоединен, отсоедин€ем его
            if (reductor._attachedBallon != null)
            {
                Debug.Log("Detaching...");
                reductor.DetachFromBallon();
            }

            // Grab
            hand.AttachObject(gameObject, startingGrabType);
            hand.HoverLock(interactable);
            currentHand = hand;

            Debug.Log("Grabbed reductor.");
        }
        // ≈сли редуктор захвачен и происходит отпускание
        else if (interactable.attachedToHand != null && isGrabEnding)
        {
            Debug.Log("Releasing reductor.");
            // Release
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
            currentHand = null;

            Debug.Log("Released regulator.");
            reductor.block = false;
        }

        // ¬заимодействие с ручкой вентил€
        /*if (interactAction.GetStateDown(handType) && IsHandNearValveHandle(hand))
        {
            regulator.ToggleValve();
        }*/
    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint broke!");
        // ћожно добавить обработку, если FixedJoint сломалс€ (например, воспроизвести звук)
    }
}
