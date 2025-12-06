using UnityEngine;
using Valve.VR.InteractionSystem;

public class DoorInteraction : MonoBehaviour
{
    private Interactable interactable;
    private CircularDrive circularDrive;
    private Rigidbody _rigidbody;
    private HingeJoint _hingeJoint;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        circularDrive = GetComponent<CircularDrive>();
        _rigidbody = GetComponent<Rigidbody>();
        _hingeJoint = GetComponent<HingeJoint>();

    }

    private void HandHoverUpdate(Hand hand)
    {
        Debug.Log(circularDrive.outAngle);
    }
}
