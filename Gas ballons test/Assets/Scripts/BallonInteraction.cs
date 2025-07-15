using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallonInteraction : MonoBehaviour
{
    private Interactable interactable;
    private Hand currentHand;
    private FixedJoint fixedJoint; // Для крепления баллона к руке

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log("Hand hovering over gas cylinder.");
    }

    private void OnHandHoverEnd(Hand hand)
    {
        Debug.Log("Hand no longer hovering over gas cylinder.");
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            // Grab
            hand.AttachObject(gameObject, startingGrabType);
            hand.HoverLock(interactable);
            currentHand = hand;

            // Добавляем FixedJoint для крепления к руке
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = hand.GetComponent<Rigidbody>();
            fixedJoint.breakForce = Mathf.Infinity; // Чтобы не сломалось при обычном взаимодействии

            Debug.Log("Grabbed gas cylinder.");
        }
        else if (isGrabEnding)
        {
            // Release
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
            currentHand = null;

            // Удаляем FixedJoint
            if (fixedJoint != null)
            {
                Destroy(fixedJoint);
                fixedJoint = null;
            }
            Debug.Log("Released gas cylinder.");
        }
    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint broke!");
        // Можно добавить обработку, если FixedJoint сломался (например, воспроизвести звук)
    }
}
