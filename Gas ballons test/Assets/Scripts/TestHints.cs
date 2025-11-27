using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class TestHints : MonoBehaviour
{
    private Interactable interactable;
    public SteamVR_Action_Boolean action;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    void OnHandHoverBegin(Hand hand)
    {
        ControllerButtonHints.ShowTextHint(hand, action, "—хватить");
    }

    void OnHandHoverEnd(Hand hand)
    {
        ControllerButtonHints.HideTextHint(hand, action);
    }

    private void HintText(SteamVR_Action_Boolean action)
    {

    }
}
