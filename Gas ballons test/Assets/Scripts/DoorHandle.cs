using UnityEngine;
using Valve.VR.InteractionSystem;

public class DoorHandle : MonoBehaviour
{
    public InteractiveDoor door;       // Ссылка на скрипт InteractiveDoor
    public float openDirection = 1f;   // Направление открытия двери (1 или -1)

    private void OnAttachedToHand(Hand hand)
    {
        door.StartRotating(openDirection);
    }

    private void OnDetachedFromHand(Hand hand)
    {
        door.StopRotating();
    }
}