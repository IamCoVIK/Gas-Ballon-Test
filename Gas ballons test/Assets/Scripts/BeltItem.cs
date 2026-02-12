using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Whisper;
using Whisper.Utils;

public class BeltItem : MonoBehaviour
{
    [SerializeField] private GameObject ReferenceItem;
    [SerializeField] private bool isReductor;
    private GameObject SpawnedItem;

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    private void HandHoverUpdate(Hand hand)
    {
        if (grabAction.GetStateDown(hand.handType))
        {
            if (!isReductor)
            {
                hand.AttachObject(ReferenceItem, GrabTypes.Grip);
                return;
            }
            if (SpawnedItem != null)
            {
                Destroy(SpawnedItem);
            }
            SpawnedItem = Instantiate(ReferenceItem);
            hand.AttachObject(SpawnedItem, GrabTypes.Grip);
        }
    }
}
