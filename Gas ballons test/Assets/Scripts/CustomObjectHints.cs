using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public enum ObjectType
{
    None,
    DetachedReductor,
    AttachedReductor,
    Ballon,
    BallonValve,
    Door,
    Ruler,
    Notebook,
    FireExt
}

public class CustomObjectHints : MonoBehaviour
{
    private Interactable interactable;

    public ObjectType objectType;
    public List<SteamVR_Action_Boolean> actions;
    public List<string> hints;

    private static Dictionary<ObjectType, int> times = new Dictionary<ObjectType, int>()
    {
        {ObjectType.None, 0},
        {ObjectType.DetachedReductor, 2},
        {ObjectType.AttachedReductor, 2},
        {ObjectType.Ballon, 2},
        {ObjectType.BallonValve, 2},
        {ObjectType.Door, 2},
        {ObjectType.Ruler, 2},
        {ObjectType.Notebook, 2},
        {ObjectType.FireExt, 2},
    };

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    void OnHandHoverBegin(Hand hand)
    {
        if (times[objectType] == 0)
        {
            return;
        }
        times[objectType] -= 1;
        for (int i = 0; i < actions.Count; i++)
        {
            ControllerButtonHints.ShowTextHint(hand, actions[i], hints[i]);
        }
    }

    void OnHandHoverEnd(Hand hand)
    {
        /*if (times[objectType] == 0)
        {
            return;
        }*/
        foreach (var action in actions)
        {
            ControllerButtonHints.HideTextHint(hand, action);
        }
    }
}
