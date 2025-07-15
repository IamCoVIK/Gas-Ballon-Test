using UnityEngine;
using Valve.VR.InteractionSystem;

public class DoorInteraction : MonoBehaviour
{
    private Interactable interactable;
    private FixedJoint fixedJoint;
    private Hand currentHand;

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log("Hand hovering over door.");
    }

    private void OnHandHoverEnd(Hand hand)
    {
        Debug.Log("Hand no longer hovering over door.");
    }


    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            // —оедин€ем дверь с рукой
            hand.AttachObject(gameObject, startingGrabType);
            hand.HoverLock(interactable);
            currentHand = hand;
            Debug.Log("Linked door to hand.");
        }
        else if (isGrabEnding)
        {
            // ќтсоедин€ем дверь от руки
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
            currentHand = null;
            Debug.Log("Released door.");
        }
    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint broken!");
        //RemoveFixedJoint(); // убираем удаление, чтобы можно было снова брать дверь
    }
}
