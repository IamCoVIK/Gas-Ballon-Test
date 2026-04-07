using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandPocketItem : MonoBehaviour
{
    [SerializeField] private GameObject ReferenceItem;
    [SerializeField] private Transform ReferenceItemOffset;
    [SerializeField] private GameObject ItemVisualisation;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Hand handP;
    [SerializeField] private HandPockets handPockets;

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    private void HandHoverUpdate(Hand hand)
    {
        if (grabAction.GetStateDown(hand.handType))
        {
            hand.AttachObject(ReferenceItem, GrabTypes.Grip, attachmentOffset: ReferenceItemOffset);
        }
    }

    public void ResetHandPoketItemPosition()
    {
        ReferenceItem.transform.position = startPoint.position;
        ReferenceItem.transform.rotation = startPoint.rotation;
    }

    private void Start()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.isTrigger = true;
        }
    }

    private void Update()
    {
        if (handPockets.activated)
        {
            ItemVisualisation.SetActive(handP.currentAttachedObject == null);
        }
    }
}
