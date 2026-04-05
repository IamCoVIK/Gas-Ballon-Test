using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ReferenceItem;
    private GameObject SpawnedItem;

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    private void HandHoverUpdate(Hand hand)
    {
        if (grabAction.GetStateDown(hand.handType))
        {
            if (SpawnedItem != null)
            {
                Destroy(SpawnedItem);
            }
            SpawnedItem = Instantiate(ReferenceItem);
            hand.AttachObject(SpawnedItem, GrabTypes.Grip);
        }
    }
}
